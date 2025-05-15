using API.DTOs;
using API.Services;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace API.Features.Users.GetById
{
    public class GetByIdUserHandler : IRequestHandler<GetByIdUserQuery, UserDto?>
    {
        private readonly UserService _userService;
        public GetByIdUserHandler(UserService userService)
        {
            _userService = userService;
        }
        public async Task<UserDto?> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetByIdAsync(request.Id);
        }
    }
}
