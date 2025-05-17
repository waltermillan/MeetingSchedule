using API.DTOs;
using API.Services;
using MediatR;

namespace API.Features.ContactTags.GetById
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