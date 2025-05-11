using Core.Entities;
using MediatR;

namespace API.Features.ContactsTag.GetById
{
    public record GetContactTagByIdQuery(Guid Id) : IRequest<ContactTag>;
}