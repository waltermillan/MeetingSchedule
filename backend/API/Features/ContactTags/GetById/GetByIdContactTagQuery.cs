using API.DTOs;
using MediatR;

namespace API.Features.ContactTags.GetById
{
    public record GetByIdContactTagQuery(Guid Id) : IRequest<ContactTagDto>;
}