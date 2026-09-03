using MediatR;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;

namespace Vehicle_Management.Dtos
{

    public record BookVehicleDto(
        DateOnly StartDate,
        TimeOnly StartTime,
        DateOnly EndDate,
        TimeOnly EndTime,
        Guid EmployeeId,
        string VehicleId,
        Fuel_Policy Fuel_Policy)
    {
        public DateTime StartDateTime => StartDate.ToDateTime(StartTime);
        public DateTime EndDateTime => EndDate.ToDateTime(EndTime);
    }
}
