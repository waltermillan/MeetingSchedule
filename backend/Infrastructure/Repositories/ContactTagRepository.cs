using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ContactTagRepository(Context context) : GenericRepository<ContactTag>(context), IContactTagRepository
    {
        public override async Task<ContactTag> GetByIdAsync(Guid id)
        {
            return await _context.ContactsTag
                              .FirstOrDefaultAsync(p => p.Id == id);
        }
        public override async Task<IEnumerable<ContactTag>> GetAllAsync()
        {
            return await _context.ContactsTag.ToListAsync();
        }
    }
}
