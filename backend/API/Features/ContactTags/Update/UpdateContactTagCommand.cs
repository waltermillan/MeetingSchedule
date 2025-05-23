using MediatR;

namespace API.Features.ContactTags.Update
{
    public record UpdateContactTagCommand(
        Guid Id,
        Guid ContactId,
        Guid TagId,
        Guid UserId
    ) : IRequest<bool>;
}