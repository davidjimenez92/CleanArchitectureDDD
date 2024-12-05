using Asp.Versioning;
using CleanArchitectureDDD.Application.Users.GetUserDapperPagination;
using CleanArchitectureDDD.Application.Users.GetUsersPagination;
using CleanArchitectureDDD.Application.Users.LoginUser;
using CleanArchitectureDDD.Application.Users.RegisterUser;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.WebApi.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.WebApi.Controllers.Users.V2;

[ApiController]
[ApiVersion(ApiVersions.V2)]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController: ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
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