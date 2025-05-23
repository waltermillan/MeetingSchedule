using API.Features.ContactTags.Update;
using Core.Interfaces;
using MediatR;

namespace API.Features.ContactTags.Update
{
    public class UpdateContactTagHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateContactTagCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(UpdateContactTagCommand request, CancellationToken cancellationToken)
        {
            var contactTag = await _unitOfWork.ContactTags.GetByIdAsync(request.Id);
            
            if (contactTag is null) 
                return false;

            DateTime now = DateTime.UtcNow;

            contactTag.ContactId = request.ContactId;
            contactTag.TagId = request.TagId;
            contactTag.UpdatedAt = now;
            contactTag.UserId = request.UserId;

            _unitOfWork.ContactTags.Update(contactTag);
            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}