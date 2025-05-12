using API.Features.ContactsTag.Create;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace API.Features.ContactsTag.CreateContactTag
{
    public class CreateContactTagHandler : IRequestHandler<CreateContactTagCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateContactTagHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateContactTagCommand request, CancellationToken cancellationToken)
        {

            var contactTag = new ContactTag
            {
                ContactId = request.ContactId,
                TagId = request.TagId
            };

            _unitOfWork.ContactsTag.Add(contactTag);
            await _unitOfWork.SaveAsync(cancellationToken);

            return contactTag.Id;
        }
    }
}
