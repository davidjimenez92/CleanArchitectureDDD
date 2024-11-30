using CleanArchitectureDDD.Application.Vehicles.SearchVehicles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.WebApi.Controllers.Vehicles;

[ApiController]
[Route("api/vehicles")]
public class VehiclesController: ControllerBase
{
    private readonly ILogger<VehiclesController> _logger;
    private readonly ISender _sender;

    public VehiclesController(ILogger<VehiclesController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [Authorize]
    [HttpGet("search")]
    public async Task<IActionResult> SearchVehiclesAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        var query = new SearchVehiclesQuery(startDate, endDate);
        var result = await _sender.Send(query, cancellationToken);
        
        return Ok(result.Value); 
    }
}