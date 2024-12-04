using CleanArchitectureDDD.Application.Users.GetUserDapperPagination;
using CleanArchitectureDDD.Application.Users.GetUsersPagination;
using CleanArchitectureDDD.Application.Users.LoginUser;
using CleanArchitectureDDD.Application.Users.RegisterUser;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.WebApi.Controllers.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController: ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("login")]
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

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = new RegisterUserCommand(request.Email, request.Name, request.Surname, request.Password);
        var resutl = await _sender.Send(command, cancellationToken);

        if (resutl.IsFailure)
        {
            return Unauthorized(resutl.Error);
        }
        
        return Ok(resutl.Value);
    }

    [AllowAnonymous]
    [HttpGet("getPagination", Name = "PaginationUsers")]
    [ProducesResponseType(typeof(PaginationResult<User, UserId>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResults<User, UserId>>> GetPaginationUsers([FromQuery] GetUserPaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        var results = await _sender.Send(paginationQuery, cancellationToken);
        return Ok(results);
    }

    [AllowAnonymous]
    [HttpGet("getPaginationDapper", Name = "GetPaginationDapper")]
    [ProducesResponseType(typeof(PagedDapperResults<UserPaginationData>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedDapperResults<UserPaginationData>>> GetPaginationDapper(
        [FromQuery] GetUsersDapperPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var results = await _sender.Send(query, cancellationToken);
        return Ok(results);
    }
}