using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositiores;


public class BlogPostLikeRepository: IBlogPostLikeRepository
{
    private readonly BloggieDbContext _context;

    public BlogPostLikeRepository(BloggieDbContext context)
    {
        _context = context;
    }
    public async Task<int> GetTotalLikes(Guid blogPostId)
    {
       return await _context.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId); 
    }

    public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
    {
       await _context.BlogPostLike.AddAsync(blogPostLike);
       await _context.SaveChangesAsync();
       return blogPostLike;
    }

    public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
    {
       return await _context.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
    }
}