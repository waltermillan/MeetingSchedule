using MediatR;

namespace API.Features.Contacts.Delete
{
    public record DeleteContactCommand(Guid Id) : IRequest<bool>;
}