using MediatR;

namespace API.Features.Users.Create
{
    public record CreateUserCommand(
        string Name,
        string UserName,
        string Password
    ) : IRequest<Guid>;
}
