using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace API.Features.Contacts.GetAll
{
    public class GetAllContactsHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<Contact>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllContactsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Contact>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Contacts.GetAllAsync();
        }
    }
}