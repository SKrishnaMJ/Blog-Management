using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositiores;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly BloggieDbContext _context;

    public BlogPostRepository(BloggieDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        //The include is used to retrive that specific property which has a relationship with this table
        return await _context.BlogPosts.Include(x => x.Tags).ToListAsync();
    }

    public async Task<BlogPost?> GetAsync(Guid id)
    {
        return await _context.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        
    }

    public async Task<BlogPost> AddAsync(BlogPost blogPost)
    {
        await _context.BlogPosts.AddAsync(blogPost);
        await _context.SaveChangesAsync();
        return blogPost;
        
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
    {
        var exisitngBlog = await _context.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
        if (exisitngBlog != null)
        {
            exisitngBlog.Id = blogPost.Id;
            exisitngBlog.Heading = blogPost.Heading;
            exisitngBlog.PageTitle = blogPost.PageTitle;
            exisitngBlog.Content = blogPost.Content;
            exisitngBlog.ShortDescription = blogPost.ShortDescription;
            exisitngBlog.Author = blogPost.Author;
            exisitngBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
            exisitngBlog.UrlHandle = blogPost.UrlHandle;
            exisitngBlog.Visible = blogPost.Visible;
            exisitngBlog.PublishDate = blogPost.PublishDate;
            exisitngBlog.Tags = blogPost.Tags;
            
            await _context.SaveChangesAsync();
            return exisitngBlog;
            
        }
        return null;
    }

    public async Task<BlogPost?> DeleteAsync(Guid id)
    {
        var existingBlogPost = await _context.BlogPosts.FindAsync(id);

        if (existingBlogPost != null)
        {
            _context.BlogPosts.Remove(existingBlogPost);
            await _context.SaveChangesAsync();
            return existingBlogPost;
        }
        return null;
    }
}