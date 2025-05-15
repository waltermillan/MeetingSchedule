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
    public class UserRepository(Context context) : GenericRepository<User>(context), IUserRepository
    {
        public override async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users
                               .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByNameAsync(string userName)
        {
            return await _context.Users
                               .FirstOrDefaultAsync(p => p.UserName.ToUpper() == userName.ToUpper());
        }
    }
}
