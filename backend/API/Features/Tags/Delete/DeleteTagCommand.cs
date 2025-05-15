using MediatR;

namespace API.Features.Tags.Delete
{
    public record DeleteTagCommand(Guid Id) : IRequest<bool>;
}