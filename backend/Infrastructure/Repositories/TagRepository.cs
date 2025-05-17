using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TagRepository(Context context) : GenericRepository<Tag>(context), ITagRepository
    {
        public override async Task<Tag> GetByIdAsync(Guid id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(p => p.Id == id);
            return tag ?? throw new KeyNotFoundException($"Tag with ID {id} not found.");
        }
        public override async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }
    }
}
