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
            return await _context.Contacts
                              .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _context.Contacts.ToListAsync();
        }
    }
}
