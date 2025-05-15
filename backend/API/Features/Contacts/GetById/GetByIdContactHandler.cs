using Core.Interfaces;
using MediatR;
using Core.Entities;

namespace API.Features.Contacts.GetById
{
    public class GetByIdContactHandler : IRequestHandler<GetByIdContactQuery, Contact?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdContactHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Contact?> Handle(GetByIdContactQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Contacts.GetByIdAsync(request.Id);
        }
    }
}