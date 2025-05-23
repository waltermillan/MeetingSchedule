using API.Features.ContactTags.Create;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace API.Features.ContactTags.Create
{
    public class CreateContactTagHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateContactTagCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Guid> Handle(CreateContactTagCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;

            var contactTag = new ContactTag
            {
                ContactId = request.ContactId,
                TagId = request.TagId,
                CreatedAt = now,
                UpdatedAt = now,
                UserId = request.UserId
            };

            _unitOfWork.ContactTags.Add(contactTag);
            await _unitOfWork.SaveAsync(cancellationToken);

            return contactTag.Id;
        }
    }
}
