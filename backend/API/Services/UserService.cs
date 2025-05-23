using API.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace API.Services
{
    public class UserService(IUserRepository userRepository)
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserDto> GetByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
               ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            if (users is null || !users.Any())
                return [];

            return users.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                CreatedAt = user.CreatedAt,

            });
        }

        public async Task<User> GetByNameAsync(string userName)
        {
            var user = await _userRepository.GetByNameAsync(userName)
                ?? throw new KeyNotFoundException($"User with Name {userName} not found.");

            return new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                Password = user.Password
            };
        }
    }
}