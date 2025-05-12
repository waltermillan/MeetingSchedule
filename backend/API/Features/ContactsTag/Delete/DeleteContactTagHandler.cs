using Core.Interfaces;
using MediatR;

namespace API.Features.ContactsTag.Delete
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
            var contactTag = await _unitOfWork.ContactsTag.GetByIdAsync(request.Id);
            if (contactTag == null) return false;

            _unitOfWork.ContactsTag.Remove(contactTag);
            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}