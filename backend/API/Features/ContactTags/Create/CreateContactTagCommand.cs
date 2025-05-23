using MediatR;

namespace API.Features.ContactTags.Create
{
    public record CreateContactTagCommand(
        Guid ContactId,
        Guid TagId,
        Guid UserId
    ) : IRequest<Guid>;
}
