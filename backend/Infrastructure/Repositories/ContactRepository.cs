using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ContactRepository(Context context) : GenericRepository<Contact>(context), IContactRepository
    {
        public override async Task<Contact> GetByIdAsync(Guid id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(p => p.Id == id);
            return contact ?? throw new KeyNotFoundException($"Contact with ID {id} not found.");
        }

        public override async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _context.Contacts.ToListAsync();
        }
    }
}
