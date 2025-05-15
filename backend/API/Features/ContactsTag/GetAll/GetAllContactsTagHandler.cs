using API.DTOs;
using API.Services;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace API.Features.ContactsTag.GetAll
{
    public class GetAllContactsTagHandler : IRequestHandler<GetAllContactsTagQuery, IEnumerable<ContactTagDto>>
    {
        private readonly ContactTagService _contactTagDtoService;

        public GetAllContactsTagHandler(ContactTagService contactTagDtoService)
        {
            _contactTagDtoService = contactTagDtoService;
        }

        public async Task<IEnumerable<ContactTagDto>> Handle(GetAllContactsTagQuery request, CancellationToken cancellationToken)
        {
            return await _contactTagDtoService.GetAllAsync();
        }
    }
}
