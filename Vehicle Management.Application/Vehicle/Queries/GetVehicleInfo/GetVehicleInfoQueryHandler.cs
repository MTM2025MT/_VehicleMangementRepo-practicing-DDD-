using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.IRepositories;
namespace Vehicle_Management.Application.Vehicle.Queries.GetVehicleInfo
{
    public class GetVehicleInfoQueryHandler : IRequestHandler<GetVehicleInfoQuery, VehicleInfoDto>
    {
        private IVehicleRepository _vehicleRepository;
        public GetVehicleInfoQueryHandler(IVehicleRepository vehicleRepository) {

            _vehicleRepository=vehicleRepository;
        }
        public async Task<VehicleInfoDto> Handle(GetVehicleInfoQuery query, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(query.Vehicle_Id);

            if (vehicle == null)
                throw new Exception("Vehicle not found.");

            return new VehicleInfoDto(
                Id: vehicle.License_Plate,
                Model: vehicle._classification,
                IsAvailable: vehicle.IsAvailable(),
                Odometer: vehicle.Odometer
            );
        }
    }
}
