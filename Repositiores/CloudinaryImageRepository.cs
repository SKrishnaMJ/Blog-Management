using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Bloggie.Web.Repositiores;

public class CloudinaryImageRepository : IImageRepository
{
    private readonly IConfiguration _config;
    private readonly Account _account;

    public CloudinaryImageRepository(IConfiguration config)
    {
        _config = config;
        _account = new Account(
            config.GetSection("Cloudinary")["CloudName"],
            config.GetSection("Cloudinary")["ApiKey"],
            config.GetSection("Cloudinary")["ApiSecret"]);
    }
    public async Task<string> UploadAsync(IFormFile file)
    {
        var client = new Cloudinary(_account);

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            DisplayName = file.FileName
        };
        
        var uploadResult = await client.UploadAsync(uploadParams);

        if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return uploadResult.SecureUri.ToString();
        }
        
        return null;
    }
}