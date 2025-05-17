using API.Features.Tags.Create;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace API.Features.Tags.CreateTag
{
    public class CreateTagHandler : IRequestHandler<CreateTagCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTagHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var tag = new Tag
            {
                Name = request.Name,
                Color = request.Color
            };

            _unitOfWork.Tags.Add(tag);
            await _unitOfWork.SaveAsync(cancellationToken);

            return tag.Id;
        }
    }
}
