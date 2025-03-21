using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositiores;

public class BlogPostCommentRepository: IBlogPostCommentRepository
{
    private readonly BloggieDbContext _dbContext;

    public BlogPostCommentRepository(BloggieDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<BlogPostComment> AddAsync(BlogPostComment comment)
    {
       await _dbContext.AddAsync(comment);
       await _dbContext.SaveChangesAsync();
       return comment;
    }

    public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
    {
        return await _dbContext.BlogPostComment.Where(c => c.BlogPostId == blogPostId).ToListAsync();
    }
}