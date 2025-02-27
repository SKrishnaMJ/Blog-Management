namespace Bloggie.Web.Repositiores;

public interface IImageRepository
{
    Task<string> UploadAsync(IFormFile file);
}