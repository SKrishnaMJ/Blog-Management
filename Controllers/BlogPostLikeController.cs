using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositiores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostLikeController : ControllerBase
{
    private readonly IBlogPostLikeRepository _repository;

    public BlogPostLikeController(IBlogPostLikeRepository repository )
    {
        _repository = repository;
    }
    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> AddLike([FromBody] AddLikeRequest request)
    {
        var model = new BlogPostLike
        {
            BlogPostId = request.BlogPostId,
            UserId = request.UserId
        };
        
        await _repository.AddLikeForBlog(model);
        
        return Ok();
    }

    [HttpGet]
    [Route("{blogPostId:Guid}/totalLikes")]
    public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId)
    {
        var totalLikes = await _repository.GetTotalLikes(blogPostId);
        return Ok(totalLikes);
    }
}