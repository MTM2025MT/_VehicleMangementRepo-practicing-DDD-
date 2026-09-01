using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.IRepositories;
using BookingAggregateDomain= Vehicle_Management.Domain.Aggregates.BookingAggregate;
namespace Vehicle_Management.Application.BookingManagement.Commands.CancelBooking
{
    internal class CancelBookingCommandHandler:IRequestHandler<CancelBookingCommand,bool>
    {
        private IBookingRepository bookingRepository;
        public CancelBookingCommandHandler(IBookingRepository BookingRepository, IVehicleRepository VehicleRepository)
        {
            bookingRepository = BookingRepository;
        }
        public async Task<bool> Handle(CancelBookingCommand Command, CancellationToken cancellationToken)
        {
            var booking = await bookingRepository.GetByIdAsync(Command.Booking_Id);
            if (booking is null)
            {
                throw new Exception("that booking in not valid ");
            }
            booking.CancleBooking();

            var result =await bookingRepository.UpdateAsync(booking);
             if (result is BookingAggregateDomain.Booking)
            {
                return true;
            }
            return false;
        }


    }
}
