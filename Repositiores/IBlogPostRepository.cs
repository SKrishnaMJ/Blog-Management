using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositiores;

public interface IBlogPostRepository
{
    Task<IEnumerable<BlogPost>> GetAllAsync();
    
    Task<BlogPost?> GetAsync(Guid id);
    
    Task<BlogPost> AddAsync(BlogPost blogPost);
    
    Task<BlogPost?> UpdateAsync(BlogPost blogPost);
    
    Task<BlogPost?> DeleteAsync(Guid id);
}