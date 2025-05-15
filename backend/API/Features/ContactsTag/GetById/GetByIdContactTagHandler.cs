using Core.Interfaces;
using MediatR;
using Core.Entities;
using API.DTOs;
using API.Services;

namespace API.Features.ContactsTag.GetById
{
    public class GetByIdContactTagHandler : IRequestHandler<GetByIdContactTagQuery, ContactTagDto?>
    {
        private readonly ContactTagService _contactTagDtoService;

        public GetByIdContactTagHandler(ContactTagService contactTagDtoService)
        {
            _contactTagDtoService = contactTagDtoService;
        }

        public async Task<ContactTagDto?> Handle(GetByIdContactTagQuery request, CancellationToken cancellationToken)
        {
            return await _contactTagDtoService.GetByIdAsync(request.Id);
        }
    }
}