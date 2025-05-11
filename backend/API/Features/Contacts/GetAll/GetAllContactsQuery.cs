using Core.Entities;
using MediatR;

namespace API.Features.Contacts.GetAll
{
    public record GetAllContactsQuery() : IRequest<IEnumerable<Contact>>;
}