using API.Features.Contacts.Create;
using Core.Entities;
using Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Features.Contacts.CreateContact
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
                UpdatedAt = now
            };

            _unitOfWork.Contacts.Add(contact);
            await _unitOfWork.SaveAsync(cancellationToken);

            return contact.Id;
        }
    }
}
