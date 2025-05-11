using Core.Interfaces;
using MediatR;
using Core.Entities;

namespace API.Features.Tags.GetById;

public class GetTagByIdHandler : IRequestHandler<GetTagByIdQuery, Tag?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTagByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Tag?> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Tags.GetByIdAsync(request.Id);
    }
}
