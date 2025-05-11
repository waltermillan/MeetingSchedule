using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace API.Features.ContactsTag.GetAll
{
    public class GetAllContactsTagHandler : IRequestHandler<GetAllContactsTagQuery, IEnumerable<ContactTag>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllContactsTagHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ContactTag>> Handle(GetAllContactsTagQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ContactsTag.GetAllAsync();
        }
    }
}
