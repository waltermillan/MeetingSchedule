using Core.Interfaces;
using MediatR;

namespace API.Features.Tags.Update;

public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTagHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(request.Id);
        if (tag == null) return false;

        tag.Name = request.Name;
        tag.Color = request.Color;

        _unitOfWork.Tags.Update(tag);
        await _unitOfWork.SaveAsync(cancellationToken);

        return true;
    }
}
