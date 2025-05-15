using MediatR;

namespace API.Features.Users.Delete
{
    public record DeleteUserCommand(Guid Id) : IRequest<bool>;
}