using MediatR;

namespace API.Features.Tags.Create
{
    public record CreateTagCommand(
        string Name,
        string Color,
        Guid? UserId
    ) : IRequest<Guid>;
}
