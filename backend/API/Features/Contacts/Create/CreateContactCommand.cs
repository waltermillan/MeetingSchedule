using MediatR;

namespace API.Features.Contacts.Create
{
    /*
  
        This indicates that CreateContactCommand is a request that will be sent to a handler.

        IRequest<T> is part of MediatR and represents an order or command.
    
    */
    public record CreateContactCommand(
        string Name,
        string Email,
        string Phone,
        string Address
    ) : IRequest<Guid>;
}
