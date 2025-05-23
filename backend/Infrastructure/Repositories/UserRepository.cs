using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository(Context context) : GenericRepository<User>(context), IUserRepository
    {
        public override async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);
            return user ?? throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByNameAsync(string userName)
        {
            var user = await _context.Users
                                     .FirstOrDefaultAsync(p => p.UserName != null &&
                                                                p.UserName.ToUpper().Equals(userName.ToUpper()));

            return user ?? throw new KeyNotFoundException($"User with username '{userName}' not found.");
        }
    }
}
