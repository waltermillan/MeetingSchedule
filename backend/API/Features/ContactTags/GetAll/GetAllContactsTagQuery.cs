using API.DTOs;
using MediatR;

namespace API.Features.ContactTags.GetAll
{
    public record GetAllContactTagsQuery() : IRequest<IEnumerable<ContactTagDto>>;
}