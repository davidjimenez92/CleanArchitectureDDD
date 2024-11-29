using CleanArchitectureDDD.Application.Users.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.WebApi.Controllers.Users;

[ApiController]
[Route("api/users")]
public class UsersController: ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }
        
        return Ok(result.Value);
    }
}