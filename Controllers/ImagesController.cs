using System.Net;
using Bloggie.Web.Repositiores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImageRepository _imageRepository;

    public ImagesController(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }
    [HttpPost]
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        //To call a repo
        var imageUrl = await _imageRepository.UploadAsync(file);

        if (imageUrl == null)
        {
            return Problem("Something went wrong", null, (int)HttpStatusCode.InternalServerError);
        }
        return new JsonResult(new { link = imageUrl });
    }
}