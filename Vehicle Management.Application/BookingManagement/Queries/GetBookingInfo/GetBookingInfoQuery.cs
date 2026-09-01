using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;

namespace Vehicle_Management.Application.BookingManagement.Queries.GetBookingInfo
{
    public record BookingInfoDto(
          Guid Employee_Id,
          string Vehicle_id,
          string bookingstatus,
          string Fuel_policy,
          DateTime start_time,
          DateTime finish_time,
          string  vehicle_model
        );
    public record GetBookingInfoQuery(int id):IRequest<BookingInfoDto>;
}
