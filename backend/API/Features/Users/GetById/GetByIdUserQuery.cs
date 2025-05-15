using API.DTOs;
using MediatR;

namespace API.Features.Users.GetById
{
    public record GetByIdUserQuery(Guid Id) : IRequest<UserDto?>;
}
