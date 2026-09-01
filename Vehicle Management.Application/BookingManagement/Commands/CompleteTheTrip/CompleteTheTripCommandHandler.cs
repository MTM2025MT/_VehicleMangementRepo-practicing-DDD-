using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;
using Vehicle_Management.Domain.IRepositories;

namespace Vehicle_Management.Application.BookingManagement.Commands.CompleteTheTrip
{
    internal class CompleteTheTripCommandHandler : IRequestHandler<CompleteTheTripCommand, bool>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork unitOfWork;
        public CompleteTheTripCommandHandler(
            IBookingRepository bookingRepository, IUnitOfWork UnitOfWork)
        {
            _bookingRepository = bookingRepository;
            unitOfWork=UnitOfWork;
        }


        public async Task<bool> Handle(CompleteTheTripCommand command, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(command.Booking_Id);

            if (booking is  null)
            {
                throw new InvalidOperationException("Booking id is invalid or wrong.");
            }

            try
            {

                booking.CompleteTrip(command.meters);
                await _bookingRepository.UpdateAsync(booking);
            }
            catch (Exception ex)
            {
                throw new Exception("the opertion  complete the trip  failed ", ex);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }


    }
}
