using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositiores;

public class TagRepository : ITagRepository
{
    private readonly BloggieDbContext _context;

    public TagRepository(BloggieDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _context.Tags.ToListAsync();
        
    }

    public async Task<Tag?> GetAsync(Guid id)
    {
        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
        return tag;
    }

    public async Task<Tag> AddAsync(Tag tag)
    {
        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();
        return tag;
    }

    public async Task<Tag?> UpdateAsync(Tag tag)
    {
        var existingTag = await _context.Tags.FindAsync(tag.Id);

        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;
            
            await _context.SaveChangesAsync();
            
            return existingTag;
        }
        
        return null;
    }

    public async Task<Tag?> DeleteAsync(Guid id)
    {
        var exisitingTag = await _context.Tags.FindAsync(id);
        if (exisitingTag != null)
        {
            _context.Tags.Remove(exisitingTag);
            await _context.SaveChangesAsync();
            return exisitingTag;
        }
        else
        {
            return null;
        }
    }
}