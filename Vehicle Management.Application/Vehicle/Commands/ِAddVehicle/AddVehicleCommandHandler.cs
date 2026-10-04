using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;
 
using Vehicle_Management.Domain.IRepositories;

namespace Vehicle_Management.Application.Vehicle.Commands._ِAddVehicle
{
    internal class AddVehicleCommandHandler : IRequestHandler<AddVehicleCommand, Unit>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private IUnitOfWork unitOfWork;

        public AddVehicleCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
        {
            _vehicleRepository = vehicleRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddVehicleCommand command, CancellationToken cancellationToken)
        {
            var vehicle =  Vehicle_Management.Domain.Aggregates.VehicleAggregate.
            Vehicle.Create(command.LicensePlate, command.Classification, command.Odometer);

            try
            {
                await _vehicleRepository.AddAsync(vehicle);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
