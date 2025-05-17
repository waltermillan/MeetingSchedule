using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace API.Features.Users.Create
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        public CreateUserHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }
        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                UserName = request.UserName,
                Password = HashPassword(request.Password),
            };
            _unitOfWork.Users.Add(user);
            await _unitOfWork.SaveAsync(cancellationToken);
            return user.Id;
        }
        private string HashPassword(string password)
        {
            return password = _passwordHasher.HashPassword(password); 
        }
    }
}
