using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    protected readonly IMapper Mapper;
    protected readonly ILogger<ApiController> Logger;

    public ApiController(IMapper mapper, ILogger<ApiController> logger)
    {
        Mapper = mapper;
        Logger = logger;
    }
}