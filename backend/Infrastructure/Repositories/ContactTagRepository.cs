using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ContactTagRepository(Context context) : GenericRepository<ContactTag>(context), IContactTagRepository
    {
        public override async Task<ContactTag> GetByIdAsync(Guid id)
        {
            var contactTag = await _context.ContactTags.FirstOrDefaultAsync(p => p.Id == id);
            return contactTag ?? throw new KeyNotFoundException($"contact-tag with ID {id} not found.");
        }
        public override async Task<IEnumerable<ContactTag>> GetAllAsync()
        {
            return await _context.ContactTags.ToListAsync();
        }
    }
}
