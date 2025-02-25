using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bloggie.Web.Controllers;

public class AdminTagsController : Controller
{
    private readonly BloggieDbContext _context;
    
    public AdminTagsController(BloggieDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddTagRequest addTagRequest)
    {
        if (ModelState.IsValid)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("List");
        }
        else
        {
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var tags = await _context.Tags.ToListAsync();
        return View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
        if (tag != null)
        {
            var editTagRequest = new EditTagRequest
            {
                Id = tag.Id,
                Name = tag.Name,
                DisplayName = tag.DisplayName
            };
   
            return View(editTagRequest);
        }
        
        return View(null);


    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
    {
        var tag = new Tag
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DisplayName
        };
        var existingTag = await _context.Tags.FindAsync(tag.Id);
        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
        return RedirectToAction("Edit", new { id = editTagRequest.Id });

    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
    {
        var tag = await _context.Tags.FindAsync(editTagRequest.Id);
        if (tag != null)
        {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("List");
        }
        
        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }
}

