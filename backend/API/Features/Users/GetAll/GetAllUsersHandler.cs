using API.DTOs;
using API.Services;
using MediatR;

namespace API.Features.Users.GetAll
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly UserService _userService;
        public GetAllUsersHandler(UserService userService)
        {
            _userService = userService;
        }
        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetAllAsync();
        }
    }
}
