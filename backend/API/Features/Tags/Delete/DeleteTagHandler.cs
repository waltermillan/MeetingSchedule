using Core.Interfaces;
using MediatR;

namespace API.Features.Tags.Delete;

public class DeleteTagHandler : IRequestHandler<DeleteTagCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTagHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(request.Id);
        
        if (tag is null) 
            return false;

        _unitOfWork.Tags.Remove(tag);
        await _unitOfWork.SaveAsync(cancellationToken);

        return true;
    }
}
