using Core.Interfaces;
using MediatR;
using Core.Entities;

namespace API.Features.Contacts.GetById
{
    public class GetContactByIdHandler : IRequestHandler<GetContactByIdQuery, Contact?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetContactByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Contact?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Contacts.GetByIdAsync(request.Id);
        }
    }
}