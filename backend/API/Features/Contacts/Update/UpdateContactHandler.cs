using Core.Interfaces;
using MediatR;

namespace API.Features.Contacts.Update
{
    public class UpdateContactHandler : IRequestHandler<UpdateContactCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateContactHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _unitOfWork.Contacts.GetByIdAsync(request.Id);

            if (contact is null) 
                return false;

            contact.Name = request.Name;
            contact.Email = request.Email;
            contact.Phone = request.Phone;
            contact.Address = request.Address;
            contact.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Contacts.Update(contact);
            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}