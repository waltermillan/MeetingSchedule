using Core.Interfaces;
using MediatR;
using Core.Entities;

namespace API.Features.ContactsTag.GetById
{
    public class GetContactTagByIdHandler : IRequestHandler<GetContactTagByIdQuery, ContactTag?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetContactTagByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ContactTag?> Handle(GetContactTagByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ContactsTag.GetByIdAsync(request.Id);
        }
    }
}