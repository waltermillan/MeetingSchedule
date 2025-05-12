using Core.Interfaces;
using MediatR;

namespace API.Features.Contacts.Delete
{
    public class DeleteContactHandler : IRequestHandler<DeleteContactCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteContactHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _unitOfWork.Contacts.GetByIdAsync(request.Id);
            if (contact == null) return false;

            _unitOfWork.Contacts.Remove(contact);
            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}