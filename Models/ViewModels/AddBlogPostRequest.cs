using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Models.ViewModels;

public class AddBlogPostRequest
{
    public string Heading { get; set; }
        
    public string PageTitle { get; set; }
        
    public string Content { get; set; }
        
    public string ShortDescription { get; set; }
        
    public string FeaturedImageUrl { get; set; }
        
    public string UrlHandle { get; set; }
        
    public DateTime PublishDate { get; set; }
        
    public string Author { get; set; }
        
    public bool Visible { get; set; }
    
    
    //Display Tags (here select list item is because we want to display drop down in our views which is a select tag)
    public IEnumerable<SelectListItem> Tags { get; set; }
    
    //Collect Tag
    public string[] SelectedTags { get; set; } = Array.Empty<string>();
    
}