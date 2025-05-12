using API.DTOs;
using Core.Entities;
using MediatR;

namespace API.Features.ContactsTag.GetById
{
    public record GetContactTagByIdQuery(Guid Id) : IRequest<ContactTagDTO>;
}