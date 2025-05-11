using MediatR;

namespace API.Features.ContactsTag.Update
{
    public record UpdateContactTagCommand(
        Guid Id,
        Guid ContactId,
        Guid TagId
    ) : IRequest<bool>;
}