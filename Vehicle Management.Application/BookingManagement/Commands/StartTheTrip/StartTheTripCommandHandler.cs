using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;
using Vehicle_Management.Domain.IRepositories;

namespace Vehicle_Management.Application.BookingManagement.Commands.StartTheTrip
{
    internal class StartTheTripCommandHandler : IRequestHandler<StartTheTripCommand, bool>
    {
        private IBookingRepository bookingRepository;
        private IVehicleRepository vehicleRepository;
        private readonly IUnitOfWork unitOfWork;

        public StartTheTripCommandHandler(IUnitOfWork unitOfWork, IBookingRepository BookingRepository, IVehicleRepository VehicleRepository)
        {
            bookingRepository = BookingRepository;
            vehicleRepository = VehicleRepository;
        }
        public async Task<bool> Handle(StartTheTripCommand command, CancellationToken cancellationToken)
        {
            var booking = await bookingRepository.GetByIdAsync(command.Booking_Id);

            if (booking == null)
            {
                throw new Exception("this is id is invalid or wrong");
            }
            var vehicle = await vehicleRepository.GetByIdAsync(booking.Vehicle_id);
            try
            {
                booking = booking.StartTrip();
                vehicle.MoveToTrip();

                await bookingRepository.UpdateAsync(booking);
                await vehicleRepository.UpdateAsync(vehicle);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            await unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
