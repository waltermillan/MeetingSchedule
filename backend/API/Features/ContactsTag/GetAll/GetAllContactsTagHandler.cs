using API.DTOs;
using API.Services;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace API.Features.ContactsTag.GetAll
{
    public class GetAllContactsTagHandler : IRequestHandler<GetAllContactsTagQuery, IEnumerable<ContactTagDTO>>
    {
        private readonly ContactTagService _contactTagDTOService;

        public GetAllContactsTagHandler(ContactTagService contactTagDTOService)
        {
            _contactTagDTOService = contactTagDTOService;
        }

        public async Task<IEnumerable<ContactTagDTO>> Handle(GetAllContactsTagQuery request, CancellationToken cancellationToken)
        {
            return await _contactTagDTOService.GetAllAsync();
        }
    }
}
