using System.Runtime.InteropServices.Marshalling;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositiores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminBlogPostsController : Controller
{
    private readonly ITagRepository _tagRepository;
    private readonly IBlogPostRepository _blogPostRepository;

    public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
    {
        _tagRepository = tagRepository;
        _blogPostRepository = blogPostRepository;
    }
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        //get tags from repository
        var tags = await _tagRepository.GetAllAsync();

        var model = new AddBlogPostRequest
        {
            Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
    {
        var blogPost = new BlogPost
        {
            Heading = addBlogPostRequest.Heading,
            PageTitle = addBlogPostRequest.PageTitle,
            Content = addBlogPostRequest.Content,
            ShortDescription = addBlogPostRequest.ShortDescription,
            FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
            UrlHandle = addBlogPostRequest.UrlHandle,
            PublishDate = addBlogPostRequest.PublishDate,
            Author = addBlogPostRequest.Author,
            Visible = addBlogPostRequest.Visible
            

        };
        
        //Map Tags from selected Tags
        var selectedTags = new List<Tag>();
        foreach (var tagId in addBlogPostRequest.SelectedTags)
        {
            var selectedTagIdAdGuid = Guid.Parse(tagId);
            var existingTag = await _tagRepository.GetAsync(selectedTagIdAdGuid);

            if (existingTag != null)
            {
                selectedTags.Add(existingTag);
            }
        }
        
        //Maping tags back to domain model
        blogPost.Tags = selectedTags;
        
        await _blogPostRepository.AddAsync(blogPost);
        return RedirectToAction("Add");
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var blogPosts = await _blogPostRepository.GetAllAsync();
        return View(blogPosts);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var blogPost = await _blogPostRepository.GetAsync(id);
        var tagsDomainModel = await _tagRepository.GetAllAsync();

        if (blogPost != null)
        {
            //Map Domain Model into the view model, because its good pratice not to expose the domain(databse) model to edit or add information endpoints
            var model = new EditBlogPostRequest
            {
                Id = blogPost.Id,
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishDate = blogPost.PublishDate,
                Author = blogPost.Author,
                Visible = blogPost.Visible,
                Tags = tagsDomainModel.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
            };
            return View(model);

        }
        return View(null);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
    {
        //map view model back to domain model as the repositories only deal with domain model
        var blogPostDomainModel = new BlogPost
        {
            Id = editBlogPostRequest.Id,
            Heading = editBlogPostRequest.Heading,
            PageTitle = editBlogPostRequest.PageTitle,
            Content = editBlogPostRequest.Content,
            ShortDescription = editBlogPostRequest.ShortDescription,
            FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
            UrlHandle = editBlogPostRequest.UrlHandle,
            PublishDate = editBlogPostRequest.PublishDate,
            Author = editBlogPostRequest.Author,
            Visible = editBlogPostRequest.Visible,

        };
        //Map tags into domain model
        
        var selectedTags = new List<Tag>();
        foreach (var item in editBlogPostRequest.SelectedTags)
        {
            if (Guid.TryParse(item, out var tag))
            {
               var foundTag = await _tagRepository.GetAsync(tag);

               if (foundTag != null)
               {
                   selectedTags.Add(foundTag);
               }
            }
        }
        blogPostDomainModel.Tags = selectedTags;
        
        var updatedBlog = await _blogPostRepository.UpdateAsync(blogPostDomainModel);

        if (updatedBlog != null)
        {
            return RedirectToAction("Edit");
        }
        
        return RedirectToAction("Edit");
    }

    public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
    {
        //Talk to repo to delete this blog post
        
        var deletedBlogPost = await _blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

        if (deletedBlogPost != null)
        {
            return RedirectToAction("List");
        }
        
        return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
    }
}