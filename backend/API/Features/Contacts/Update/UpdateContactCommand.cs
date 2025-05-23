using MediatR;

namespace API.Features.Contacts.Update
{
    public record UpdateContactCommand(
        Guid Id,
        string Name,
        string Email,
        string Phone,
        string Address,
        DateTime UpdatedAt,
        Guid UserId
    ) : IRequest<bool>;
}