using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Vehicle_Management.Application.BookingManagement.Commands.BookVehicle;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;
using Vehicle_Management.Domain.IRepositories;

using DomainBooking = Vehicle_Management.Domain.Aggregates.BookingAggregate.Booking;

namespace Vehicle_Management.Application.BookingManagement.Commands.BookVehicle
{
    internal class BookVehicleCommandHandler : IRequestHandler<BookVehicleCommand, int>
    {
        private IBookingRepository bookingRepository;
        private IVehicleRepository vehicleRepository;
        private IUnitOfWork unitOfWork;
        public BookVehicleCommandHandler(IUnitOfWork UnitOfwork, IBookingRepository BookingRepository, IVehicleRepository VehicleRepository)
        {
            unitOfWork = UnitOfwork;
            bookingRepository = BookingRepository;
            vehicleRepository = VehicleRepository;
        }
        public async Task<int> Handle2(BookVehicleCommand command, CancellationToken cancellationToken)
        {


            var vehicle= await vehicleRepository.GetByIdAsync(command.VehicleId);
            if (vehicle is null) 
                throw new Exception("there is no vicle with that id ");
            
            //here might be an race condtion to book an time 
            if (!vehicle.IsAvailable())
                throw new Exception("vehicle is not avilable ");
           
            
            var booking = DomainBooking.Create(
                command.Fuel_Policy,
                command.TimeStart,
                command.TimeEnd,
                command.VehicleId,
                command.EmployeeId);
            try
            {
                var result = await bookingRepository.AddAsync(booking);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
            return booking.Id;
        }
        public async Task<int> Handle(BookVehicleCommand command, CancellationToken cancellationToken)
        {
            var booking = DomainBooking.Create(
                command.Fuel_Policy,
                command.TimeStart,
                command.TimeEnd,
                command.VehicleId,
                command.EmployeeId);
            try
            {
                var result = await bookingRepository.AddAsync(booking);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return booking.Id;
        }

    }
}
/*
 
 
 BookVehicleCommandHandler
        private IBookingRepository bookingRepository;
        private IVehicleRepository vehicleRepository;
        public BookVehicleCommandHandler(IBookingRepository BookingRepository, IVehicleRepository VehicleRepository)
        {
            bookingRepository = BookingRepository;
            vehicleRepository = VehicleRepository;
        }
CreateBooking
      ↓
Booking.Create()
      ↓
Booking raises BookingForVehicleRequest
      ↓
Domain Event Handler
      ↓
Vehicle.AssignToTrip()
      ↓
Vehicle validates itself
      ↓
Save both Booking + Vehicle
      ↓
COMMIT
 */