using MediatR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.IRepositories;

namespace Vehicle_Management.Application.BookingManagement.Queries.GetBookingInfo
{
    internal class GetBookingInfoQueryHandler:IRequestHandler<GetBookingInfoQuery, BookingInfoDto>
    {
        private IBookingRepository _bookingrepository;
        private IVehicleRepository _vehicleRepository;
        public GetBookingInfoQueryHandler(IBookingRepository bookingRepository,IVehicleRepository vehicleRepository) {
            _bookingrepository=bookingRepository;
            _vehicleRepository=vehicleRepository;
        }
        public async Task<BookingInfoDto> Handle(GetBookingInfoQuery query, CancellationToken cancellationToken)
        {
            var booking = await _bookingrepository.GetByIdAsync(query.id);
            if (booking is null)
                throw new Exception("Vehicle not found.");


            var vehicle = await _vehicleRepository.GetByIdAsync(booking.Vehicle_id);

            if (vehicle is null)
                throw new Exception(" vehicle info was not found");
 
            return new BookingInfoDto(
                Employee_Id: booking.Employee_id,
                bookingstatus: booking.BookingStatus.ToString(),
                Fuel_policy: booking._fuelpolicy.ToString(),
                start_time: booking._tripwindow.Start,
                finish_time:booking._tripwindow.End,
                Vehicle_id:booking.Vehicle_id,
                vehicle_model: vehicle._classification.ToString()
            );
        }
    }
}
