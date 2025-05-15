using Core.Interfaces;
using MediatR;

namespace API.Features.Users.Delete
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            
            if (user is null)
                return false;

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.SaveAsync(cancellationToken);
            return true;
        }
    }
}
