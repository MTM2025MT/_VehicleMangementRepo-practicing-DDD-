using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vehicle_Management.Application.Vehicle.Queries.GetVehicleInfo;
using Vehicle_Management.Domain.IRepositories;

namespace Vehicle_Management.Application.Vehicle.Queries.GetAllVehiclesInfo
{
    public class GetAllVehiclesInfoQueryHandler : IRequestHandler<GetAllVehiclesInfoQuery, List<VehicleDto>>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public GetAllVehiclesInfoQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        public async Task<List<VehicleDto>> Handle(GetAllVehiclesInfoQuery query, CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.GetAllAsync();

            if (vehicles == null || !vehicles.Any())
                throw new Exception("Vehicle not found.");

            var vehiclesList = vehicles
                .Select(v => new VehicleDto(
                    v.License_Plate,
                    v._classification.ToString(),
                    v._status.ToString(),
                    v.Odometer))
                .ToList();

            return vehiclesList;
        }
    }
}
