using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Users;

namespace CleanArchitectureDDD.Application.Users.RegisterUser;

internal class RegisterUserCommandHandler_: ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler_(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistUserByEmailAsync(new Email(request.Email), cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.AlreadyExists);
        }
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var userToRegister = User.Create(
            new Name(request.Name),
            new LastName(request.Surname),
            new Email(request.Email),
            new PasswordHash(passwordHash)
            );
        
        _userRepository.AddAsync(userToRegister);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return userToRegister.Id!.Value;
    }
}