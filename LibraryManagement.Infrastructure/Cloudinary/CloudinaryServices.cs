using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace LibraryManagement.Infrastructure.Cloudinary;

public class CloudinaryServices : ICloudinaryServices
{
    private readonly IAppLogger<CloudinaryServices> _logger;
    private CloudinaryDotNet.Cloudinary Cloudinary { get; set; }
    public CloudinaryServices(IOptions<CloudinarySetting> cloudinarySetting, IAppLogger<CloudinaryServices> logger)
    {
        _logger = logger;

        Cloudinary = new CloudinaryDotNet.Cloudinary(
            new Account(
                cloudinarySetting.Value.CloudName,
                cloudinarySetting.Value.ApiKey,
                cloudinarySetting.Value.ApiSecret
            ));
        Cloudinary.Api.Secure = true;
    }
    public string GetImage(string fileName)
    {
        try
        {
            var getResourceParams = new GetResourceParams("fileName")
            {
                QualityAnalysis = true
            };
            var getResourceResult = Cloudinary.GetResource(getResourceParams);
            var resultJson = getResourceResult.JsonObj;

            // Log quality analysis score to the console
            _logger.LogInformation("{Message}", resultJson["quality_analysis"] ?? "No quality analysis");

            return resultJson["url"]?.ToString()??"https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";
        }
        catch (Exception ex)
        {
            _logger.LogError("{ExMessage}", ex.Message);
            return @"https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";
        }
    }

    public string UploadImage(string fileName)
    {
        try
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };
            
            var uploadResult = Cloudinary.Upload(uploadParams);
            _logger.LogInformation("{ResultValue}",uploadResult.JsonObj.ToString());

            return uploadResult.Url.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError("{ExMessage}", ex.Message);
            return "https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";
        }
    }
    /// <summary>
    /// Recommended to use this method
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="stream"></param>
    /// <returns></returns>
    public string UploadImage(string fileName, Stream stream)
    {
        try
        {
            var imageUploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
            };
            var result = Cloudinary.Upload(imageUploadParams);
            
            return result.Url.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError("{ExMessage}", ex.Message);
            return string.Empty;
        }
    }
}