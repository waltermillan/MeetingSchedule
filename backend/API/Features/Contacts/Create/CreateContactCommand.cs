using MediatR;

namespace API.Features.Contacts.Create
{
    public record CreateContactCommand(
        string Name,
        string Email,
        string Phone,
        string Address
    ) : IRequest<Guid>;
}
