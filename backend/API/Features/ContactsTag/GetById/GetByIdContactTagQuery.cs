using API.DTOs;
using Core.Entities;
using MediatR;

namespace API.Features.ContactsTag.GetById
{
    public record GetByIdContactTagQuery(Guid Id) : IRequest<ContactTagDto>;
}