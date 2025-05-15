using MediatR;

namespace API.Features.Users.Update
{
    public record UpdateUserCommand(
        Guid Id,
        string Name,
        string UserName,
        string Password
    ) : IRequest<bool>;
}
