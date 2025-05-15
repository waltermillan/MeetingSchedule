using API.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace API.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }



    public async Task<UserDto> GetByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
            return null;

        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Name = user.Name
        };

        return userDto;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        if (users is null || !users.Any())
            return [];

        var usersDto = new List<UserDto>();

        foreach (var item in users)
        {
            var userDto = new UserDto
            {
                Id = item.Id,
                UserName = item.UserName,
                Name = item.Name
            };

            usersDto.Add(userDto);
        }

        return usersDto;
    }

    public async Task<User> GetByNameAsync(string userName)
    {
        var user = await _userRepository.GetByNameAsync(userName);

        if (user is null)
            return null;

        var oUser = new User
        {
            Id = user.Id,
            UserName = user.UserName,
            Name = user.Name,
            Password = user.Password,
        };

        return oUser;
    }
}
