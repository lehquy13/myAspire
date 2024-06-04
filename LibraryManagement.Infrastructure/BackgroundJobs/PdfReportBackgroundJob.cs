using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Shared.Enums;
using LibraryManagement.Domain.Shared.Results;
using LibraryManagement.Domain.Specifications.Orders;
using LibraryManagement.Domain.Specifications.Users;
using LibraryManagement.Infrastructure.EmailServices;
using Microsoft.Extensions.Options;
using Quartz;

namespace LibraryManagement.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution] // Mark that this job can't be run in parallel
internal class PdfReportBackgroundJob : IJob
{
    private readonly IAppLogger<PdfReportBackgroundJob> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly IOptions<EmailSettings> _emailSettings;

    public PdfReportBackgroundJob(
        IAppLogger<PdfReportBackgroundJob> logger,
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IEmailSender emailSender,
        IOptions<EmailSettings> emailSettings
    )
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _emailSender = emailSender;
        _emailSettings = emailSettings;
    }

    private async Task<Result> GeneratePdfAsync()
    {
        await Task.CompletedTask;
        try
        {
            var orders = await _orderRepository.GetAllListAsync(
                new OrderFilterByDateSpec(null, DateTime.Now)
            );

            var completedOrderCount = orders.Count(or => or.OrderStatus == OrderStatus.Completed);
            var completedOrderSum =
                orders.Where(or => or.OrderStatus == OrderStatus.Completed).Sum(or => or.TotalPrice);

            var userTodayCount = await _userRepository.CountAsync(
                new UserFilterByDateSpec(null, DateTime.Now)
            );

            var html =
                $"<h1>Library report at {DateTime.Now:dd-MM-yyyy}</h1>" +
                $"<h2>Today's orders: {orders.Count}</h2>" +
                $"<h2>Today's paid orders: {completedOrderCount}</h2>" +
                $"<h2>Today's revenues: {completedOrderSum}</h2>" +
                $"<h2>Today's new users: {userTodayCount}</h2>" +
                $"<h2>Reported by Quy .18</h2>";

            var render = new ChromePdfRenderer();
            var pdf = render.RenderHtmlAsPdf(html);

            var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fileName = $"Library report in {DateTime.Now:dd-MM-yyyy}.pdf";

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var fullPath = Path.Combine(root, fileName);

            pdf.SaveAs(fullPath);

            var file = PdfDocument.FromFile(Path.Combine(root, fileName));

            if (file is null)
            {
                throw new Exception($"Fail to find file {fileName}");
            }

            await _emailSender.SendEmailWithAttachment(
                _emailSettings.Value.ManagerEmail,
                $"Library report in {DateTime.Now:dd-MM-yyyy}",
                html,
                fullPath,
                true);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError($"Generate pdf error: {e.Message}");
            return Result.Fail("Generate pdf error: {e.Message}");
        }
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var result = await GeneratePdfAsync();

        if (result.IsFailure)
        {
            _logger.LogError($"Generate pdf error: {result.DisplayMessage}");
            return;
        }

        _logger.LogInformation($"Generate pdf success at {DateTime.Now:dd-MM-yyyy hh:mm:ss}");
    }
}