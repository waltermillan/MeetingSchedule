using API.Features.ContactTags.Update;
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
            var contactTag = await _unitOfWork.ContactTags.GetByIdAsync(request.Id);
            
            if (contactTag is null) 
                return false;

            contactTag.ContactId = request.ContactId;
            contactTag.TagId = request.TagId;

            _unitOfWork.ContactTags.Update(contactTag);
            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}