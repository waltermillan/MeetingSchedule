using Core.Entities;
using MediatR;

namespace API.Features.Contacts.GetById
{
    public record GetByIdContactQuery(Guid Id) : IRequest<Contact>;
}