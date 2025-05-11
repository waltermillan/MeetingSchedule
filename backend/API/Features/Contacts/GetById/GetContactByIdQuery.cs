using Core.Entities;
using MediatR;

namespace API.Features.Contacts.GetById
{
    public record GetContactByIdQuery(Guid Id) : IRequest<Contact>;
}