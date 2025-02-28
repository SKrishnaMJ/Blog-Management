using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bloggie.Web.Models;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositiores;

namespace Bloggie.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly ITagRepository _tagRepository;


    public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
    {
        _logger = logger;
        _blogPostRepository = blogPostRepository;
        _tagRepository = tagRepository;
    }

    public async Task<IActionResult> Index()
    {
        //get all blogs
        var blogPosts = await _blogPostRepository.GetAllAsync();
        
        //get all tags
        var tags = await _tagRepository.GetAllAsync();
        
        //When we have two models that we want to pass to the view, we need to create a seperate viewmodel(dto) that cotains details from both models and pass this combined viewmddel to the view like below.
        var model = new HomeViewModel
        {
            BlogPosts = blogPosts,
            Tags = tags
        };
        return View(model);
        
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}