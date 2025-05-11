using MediatR;

namespace API.Features.ContactsTag.Create
{
    public record CreateContactTagCommand(
        Guid ContactId,
        Guid TagId
    ) : IRequest<Guid>;
}
