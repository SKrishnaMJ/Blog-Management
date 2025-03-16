using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositiores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

public class BlogsController : Controller
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IBlogPostLikeRepository _likesRepository;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IBlogPostCommentRepository _blogPostCommentRepository;

    public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository likesRepository, 
        SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IBlogPostCommentRepository blogPostCommentRepository)
    {
        _blogPostRepository = blogPostRepository;
        _likesRepository = likesRepository;
        _signInManager = signInManager;
        _userManager = userManager;
        _blogPostCommentRepository = blogPostCommentRepository;
    }
    // GET
    [HttpGet]
    public async Task<IActionResult> Index(string urlHandle)
    {
        var liked = false;
        var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
        var blogDetailsViewModel = new BlogDetailsViewModel();
        

        if (blogPost != null)
        {
            var totalLikes = await _likesRepository.GetTotalLikes(blogPost.Id);

            if (_signInManager.IsSignedIn(User))
            {
                var likesForBlog = await _likesRepository.GetLikesForBlog(blogPost.Id);
                
                var userId = _userManager.GetUserId(User);

                if (userId != null)
                {
                    var likeFromUSer = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                    liked = likeFromUSer != null;
                }
            }
            
            var blogComments = await _blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);

            var blogCommentsForView = new List<BlogComment>();

            foreach (var blogComment in blogComments)
            {
                blogCommentsForView.Add(new BlogComment
                {
                    Description = blogComment.Description,
                    DateAdded = blogComment.DateAdded,
                    Username = (await _userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                });
            }
            

            blogDetailsViewModel = new BlogDetailsViewModel
            {
                Id = blogPost.Id,
                Content = blogPost.Content,
                PageTitle = blogPost.PageTitle,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Heading = blogPost.Heading,
                PublishDate = blogPost.PublishDate,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Visible = blogPost.Visible,
                Tags = blogPost.Tags,
                TotalLikes = totalLikes,
                Liked = liked,
                Comments = blogCommentsForView
            };
        }
        return View(blogDetailsViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
    
    {
        if (_signInManager.IsSignedIn(User))
        {
            var domainModel = new BlogPostComment
            {
                BlogPostId = blogDetailsViewModel.Id,
                Description = blogDetailsViewModel.CommentDescription,
                UserId = Guid.Parse(_userManager.GetUserId(User)),
                DateAdded = DateTime.Now
            };
            await _blogPostCommentRepository.AddAsync(domainModel);
            return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
        }

        return Forbid();


    }
}