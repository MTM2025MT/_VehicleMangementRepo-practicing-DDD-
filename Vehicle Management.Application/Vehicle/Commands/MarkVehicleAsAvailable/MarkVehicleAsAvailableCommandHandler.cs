using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Vehicle_Management.Domain.IRepositories;

namespace Vehicle_Management.Application.Vehicle.Commands.MarkVehicleAsAvailable
{
    public class MarkVehicleAsAvailableCommandHandler : IRequestHandler<MarkVehicleAsAvailableCommand, Unit>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private IUnitOfWork unitOfWork;

        public MarkVehicleAsAvailableCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork,CancellationToken cancellationToken)
        {
            _vehicleRepository = vehicleRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(MarkVehicleAsAvailableCommand command, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(command.VehicleId);
            if (vehicle == null)
                throw new InvalidOperationException($"Vehicle not found: {command.VehicleId}");

            // domain method - adjust if your domain uses a different method/property
            vehicle.MarkAsAvilable();
            if (vehicle._status != Domain.Aggregates.VehicleAggregate.Status.Available)
                throw new  Exception("failed to update the vehicle to avilable ");

            await _vehicleRepository.UpdateAsync(vehicle);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}