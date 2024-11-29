using CleanArchitectureDDD.Application.Abstractions.Authentication;
using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Users;

namespace CleanArchitectureDDD.Application.Users.LoginUser;

internal sealed class LoginUserCommandHandler: ICommandHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(new Email(request.Email), cancellationToken);
        if(user is null)
        {
            Result.Failure<string>(UserErrors.NotFound);
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash.Value))
        {
            Result.Failure<string>(UserErrors.InvalidCredentials);
        }

        var token = await _jwtProvider.GenerateEncodedToken(user, cancellationToken);
        
        return token;
    }
}