using API.Features.ContactsTag.Update;
using Core.Interfaces;
using MediatR;

namespace API.Features.Contacts.Update
{
    public class UpdateContactTagHandler : IRequestHandler<UpdateContactTagCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateContactTagHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateContactTagCommand request, CancellationToken cancellationToken)
        {
            var contactTag = await _unitOfWork.ContactsTag.GetByIdAsync(request.Id);
            if (contactTag == null) return false;

            contactTag.ContactId = request.ContactId;
            contactTag.TagId = request.TagId;

            _unitOfWork.ContactsTag.Update(contactTag);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}