using API.DTOs;
using MediatR;

namespace API.Features.Users.GetAll
{
    public record GetAllUsersQuery() : IRequest<IEnumerable<UserDto>>;
}
