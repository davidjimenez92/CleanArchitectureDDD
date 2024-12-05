using Asp.Versioning;
using CleanArchitectureDDD.Application.Rentals.BookRental;
using CleanArchitectureDDD.Application.Rentals.GetRental;
using CleanArchitectureDDD.WebApi.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.WebApi.Controllers.Rentals.V1;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/rentals")]
public class RentalsController: ControllerBase
{
    private readonly ISender _sender;

    public RentalsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetRentalAsync(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetRentalQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        
        return result.IsIsSuccess? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> BookRentalAsync(Guid id, BookRentalRequest request, CancellationToken cancellationToken)
    {
        var command = new BookRentalCommand(request.VehicleId, request.UserId, request.StartDate, request.EndDate);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        
        return CreatedAtAction(nameof(GetRentalAsync), new { id = result.Value }, result.Value);
    }
}