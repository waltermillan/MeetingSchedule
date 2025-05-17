using Core.Entities;
using MediatR;

namespace API.Features.Tags.GetById
{
    public record GetByIdTagQuery(Guid Id) : IRequest<Tag>;
}