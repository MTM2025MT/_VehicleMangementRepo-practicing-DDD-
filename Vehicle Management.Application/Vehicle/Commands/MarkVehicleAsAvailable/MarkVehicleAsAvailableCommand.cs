using System;
using MediatR;

namespace Vehicle_Management.Application.Vehicle.Commands.MarkVehicleAsAvailable
{
    // Command to mark a vehicle as available
    public record MarkVehicleAsAvailableCommand(string VehicleId) : IRequest<MediatR.Unit>;
}