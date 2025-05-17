using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace API.Features.Tags.GetAll
{
    public class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, IEnumerable<Tag>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllTagsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Tag>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Tags.GetAllAsync();
        }
    }
}