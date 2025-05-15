using Core.Interfaces;
using MediatR;
using Core.Entities;

namespace API.Features.Tags.GetById;

public class GetByIdTagHandler : IRequestHandler<GetByIdTagQuery, Tag?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdTagHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Tag?> Handle(GetByIdTagQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Tags.GetByIdAsync(request.Id);
    }
}
