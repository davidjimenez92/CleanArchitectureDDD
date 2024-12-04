using CleanArchitectureDDD.Application.Vehicles.GetVehiclesByPagination;
using CleanArchitectureDDD.Application.Vehicles.SearchVehicles;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Entities.Permissions;
using CleanArchitectureDDD.Domain.Vehicles;
using CleanArchitectureDDD.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.WebApi.Controllers.Vehicles;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController: ControllerBase
{
    private readonly ILogger<VehiclesController> _logger;
    private readonly ISender _sender;

    public VehiclesController(ILogger<VehiclesController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }
    
    [HasPermission(PermissionEnum.ReadUser)]
    [HttpGet("search")]
    public async Task<IActionResult> SearchVehiclesAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        var query = new SearchVehiclesQuery(startDate, endDate);
        var result = await _sender.Send(query, cancellationToken);
        
        return Ok(result.Value); 
    }

    [AllowAnonymous]
    [HttpGet("getPaginated", Name = "PaginationVehicles")]
    [ProducesResponseType(typeof(PaginationResult<Vehicle, VehicleId>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResults<Vehicle, VehicleId>>> GetPaginatedVehiclesAsync([FromQuery] GetVehiclesByPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result.Value);
    }
}