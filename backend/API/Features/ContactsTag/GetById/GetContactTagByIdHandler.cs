using Core.Interfaces;
using MediatR;
using Core.Entities;
using API.DTOs;
using API.Services;

namespace API.Features.ContactsTag.GetById
{
    public class GetContactTagByIdHandler : IRequestHandler<GetContactTagByIdQuery, ContactTagDTO?>
    {
        private readonly ContactTagService _contactTagDTOService;

        public GetContactTagByIdHandler(ContactTagService contactTagDTOService)
        {
            _contactTagDTOService = contactTagDTOService;
        }

        public async Task<ContactTagDTO?> Handle(GetContactTagByIdQuery request, CancellationToken cancellationToken)
        {
            return await _contactTagDTOService.GetByIdAsync(request.Id);
        }
    }
}