using Core.Interfaces;
using MediatR;

namespace API.Features.ContactTags.Delete
{
    public class DeleteContactTagHandler : IRequestHandler<DeleteContactTagCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteContactTagHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteContactTagCommand request, CancellationToken cancellationToken)
        {
            var contactTag = await _unitOfWork.ContactTags.GetByIdAsync(request.Id);
            
            if (contactTag is null) 
                return false;

            _unitOfWork.ContactTags.Remove(contactTag);
            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}