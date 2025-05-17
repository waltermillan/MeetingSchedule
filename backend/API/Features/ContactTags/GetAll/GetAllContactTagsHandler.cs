using API.DTOs;
using API.Services;
using MediatR;

namespace API.Features.ContactTags.GetAll
{
    public class GetAllContactTagsHandler(ContactTagService contactTagDtoService) : IRequestHandler<GetAllContactTagsQuery, IEnumerable<ContactTagDto>>
    {

        public async Task<IEnumerable<ContactTagDto>> Handle(GetAllContactTagsQuery request, CancellationToken cancellationToken)
        {
            return await contactTagDtoService.GetAllAsync();
        }
    }
}
