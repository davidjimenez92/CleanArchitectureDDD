using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Vehicles;
using CleanArchitectureDDD.Domain.Vehicles.Specifications;

namespace CleanArchitectureDDD.Application.Vehicles.GetVehiclesByPagination;

internal sealed class GetVehiclesByPaginationQueryHandler: IQueryHandler<GetVehiclesByPaginationQuery, PaginationResult<Vehicle, VehicleId>>
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetVehiclesByPaginationQueryHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Result<PaginationResult<Vehicle, VehicleId>>> Handle(GetVehiclesByPaginationQuery request, CancellationToken cancellationToken)
    {
        var spec = new VehiclePaginationSpecification(request.Sort!, request.PageIndex, request.PageSize, request.Search!);
        var records = await _vehicleRepository.GetAllWithSpecificationAsync(spec, cancellationToken);

        var specCount = new VehiclePaginationCountingSpecification(request.Search!);
        var totalRecords = await _vehicleRepository.CountAsync(specCount, cancellationToken);
        
        var totalPages = (int)Math.Ceiling(Convert.ToDecimal(totalRecords) / Convert.ToDecimal(request.PageSize));

        var recordsByPage = records.Count;

        return new PaginationResult<Vehicle, VehicleId>()
        {
            Count = totalRecords,
            Data = records.ToList(),
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            ResultByPage = recordsByPage
        };
    }
}