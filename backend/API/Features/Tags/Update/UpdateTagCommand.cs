using MediatR;

namespace API.Features.Tags.Update
{
    public record UpdateTagCommand(
        Guid Id,
        string Name,
        string Color,
        Guid? UserId
    ) : IRequest<bool>;
}