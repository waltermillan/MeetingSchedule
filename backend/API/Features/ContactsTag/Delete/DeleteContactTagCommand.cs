using MediatR;

namespace API.Features.ContactsTag.Delete
{
    public record DeleteContactTagCommand(Guid Id) : IRequest<bool>;
}