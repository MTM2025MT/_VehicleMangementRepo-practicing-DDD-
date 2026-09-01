using MediatR;
using System;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;

namespace Vehicle_Management.Application.BookingManagement.Commands.BookVehicle
{
    public record BookVehicleCommand(DateTime TimeStart, DateTime TimeEnd, Guid EmployeeId, string VehicleId,Fuel_Policy Fuel_Policy) : IRequest<int>;
}
