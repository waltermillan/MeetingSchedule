using API.Features.Contacts.Create;
using Core.Entities;
using Core.Interfaces;
using MediatR;

/*
 
 Mediator: This pattern decouples the sending of a request from its handling, promoting a clean and organized architecture.

    CreateContactCommand: represents a Request.

    CreateContactHandler: is the handler that receives and handles that request.

 */

namespace API.Features.Contacts.Create
{
    public class CreateContactHandler : IRequestHandler<CreateContactCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateContactHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var contact = new Contact
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                CreatedAt = now,
                UpdatedAt = now,
                UserId = request.UserId
            };

            _unitOfWork.Contacts.Add(contact);
            await _unitOfWork.SaveAsync(cancellationToken);

            return contact.Id;
        }
    }
}
