using Core.Entities;
using MediatR;

namespace API.Features.Tags.GetById;

public record GetTagByIdQuery(Guid Id) : IRequest<Tag>;
