using Core.Interfaces;
using MediatR;

namespace API.Features.Users.Update;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.Id);

        if (user is null)
            return false;

        user.Name = request.Name;
        user.UserName = request.UserName;
        user.Password = HashPassword(request.Password);

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveAsync(cancellationToken);

        return true;
    }

    private string HashPassword(string password)
    {
        return password = _passwordHasher.HashPassword(password);
    }
}
