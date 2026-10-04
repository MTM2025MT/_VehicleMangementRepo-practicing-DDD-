using System;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;
using Vehicle_Management.Domain.Events;
using Vehicle_Management.Domain.SeedWork;

namespace Vehicle_Management.Domain.Aggregates.BookingAggregate
{
    /*
     booking management(

     --vehicle id(commen)

     Aggregate
     */
    public class Booking : Entity<int>, IAggregateRoot
    {
        public Fuel_Policy _fuelpolicy { get; private set; }
        public TripWindow _tripwindow { get; private set; }
        public TimeSpan timeframe { get; private set; }
            public Guid Employee_id { get; private set; }
        public string Vehicle_id { get; private set; }

        private BookingStatus bookingstatus;

        public BookingStatus BookingStatus
        {
            get => GetBookingStatus();
            private set => bookingstatus = value;
        }

        private Booking() { }

        private Booking(Fuel_Policy Fuel_Policy, DateTime start, DateTime end, string VehicleId, Guid EmployeeId)
        {
            _fuelpolicy = Fuel_Policy;
            _tripwindow = MakeTripWindow(start, end);
            Employee_id = EmployeeId;
            Vehicle_id = VehicleId;
            BookingStatus = BookingStatus.Scheduled;
        }

        public static Booking Create(Fuel_Policy Fuel_Policy, DateTime start, DateTime end, string Vehicle_Id, Guid EmployeeId)
        {
            var booking = new Booking(Fuel_Policy, start, end, Vehicle_Id, EmployeeId);
            booking.AddDomainEvent(new BookingForVehicleRequestEvent(Vehicle_Id));
            return booking;
        }

        public TripWindow MakeTripWindow(DateTime start, DateTime end)
        {
            if (end < DateTime.UtcNow)
                throw new Exception("can't have a booking in the past");

            return new TripWindow(start, end);
        }

        public BookingStatus GetBookingStatus()
        {
            if (bookingstatus == BookingStatus.Scheduled && _tripwindow.End < DateTime.UtcNow)
            {
                bookingstatus = BookingStatus.Cancelled;
            }

            return bookingstatus;
        }

        public void CancleBooking()
        {
            if (BookingStatus != BookingStatus.Scheduled)
                throw new Exception("only scheduled bookings can be cancelled");

            BookingStatus = BookingStatus.Cancelled;
        }

        public Booking StartTrip()
        {
            var currentStatus = GetBookingStatus();

            if (currentStatus != BookingStatus.Scheduled)
                throw new Exception($"your trip is already {currentStatus}");

            if (_tripwindow.Start > DateTime.UtcNow)
                throw new Exception($"your booking trip starts after {_tripwindow.Start - DateTime.UtcNow}");

            BookingStatus = BookingStatus.Active;
            return this;
        }

        public Booking CompleteTrip(int meters)
        {
            var currentStatus = GetBookingStatus();

            if (currentStatus != BookingStatus.Active)
                throw new Exception("the booking is not active");

            BookingStatus = BookingStatus.Completed;
            AddDomainEvent(new CompleteTheTripEvent(Vehicle_id, meters));
            return this;
        }
    }
}
