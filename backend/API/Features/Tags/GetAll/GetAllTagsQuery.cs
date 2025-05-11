using Core.Entities;
using MediatR;

namespace API.Features.Tags.GetAll;

public record GetAllTagsQuery() : IRequest<IEnumerable<Tag>>;
