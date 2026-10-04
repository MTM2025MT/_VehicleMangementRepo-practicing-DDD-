using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vehicle_Management.Domain.IRepositories;

namespace Vehicle_Management.Application.Vehicle.Commands.SendToMaintenance
{
    public class SendToMaintenanceCommandHandler : IRequestHandler<SendToMaintenaceCommand, bool>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private IUnitOfWork unitOfWork;

        public SendToMaintenanceCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
        {
            _vehicleRepository = vehicleRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(SendToMaintenaceCommand command, CancellationToken cancellationToken)
        {
            // replace VehicleId with the actual property name on your command
            var vehicle = await _vehicleRepository.GetByIdAsync(command.Vehicle_id);
            if (vehicle == null)
                throw new InvalidOperationException($"Vehicle not found: {command.Vehicle_id}");

            vehicle.MoveToMaintenance();
            await _vehicleRepository.UpdateAsync(vehicle);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return false;
            if (vehicle._status != Domain.Aggregates.VehicleAggregate.Status.Maintenance) return false;
            return true;
        }
    }
}
