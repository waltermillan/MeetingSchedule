using API.DTOs;
using Core.Entities;
using MediatR;

namespace API.Features.ContactsTag.GetAll
{
    public record GetAllContactsTagQuery() : IRequest<IEnumerable<ContactTagDTO>>;
}