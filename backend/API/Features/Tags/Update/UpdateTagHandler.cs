using Core.Interfaces;
using MediatR;

namespace API.Features.Tags.Update
{
    public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTagHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(request.Id);

            if (tag is null)
                return false;

            DateTime now = DateTime.UtcNow;

            tag.Name = request.Name;
            tag.Color = request.Color;
            tag.UserId = request.UserId;
            tag.UpdatedAt = now;

            _unitOfWork.Tags.Update(tag);
            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}