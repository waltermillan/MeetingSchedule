using MediatR;

namespace API.Features.ContactTags.Delete
{
    public record DeleteContactTagCommand(Guid Id) : IRequest<bool>;
}