using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class Booking:  Entity, IAggregateRoot
    {
        public Fuel_Policy _fuelpolicy { get; private set; }
        public TripWindow _tripwindow { get; private set; }
        public TimeSpan timeframe { get; private set; }
        public Guid  Employee_id { get; private set; }
        public string Vehicle_id { get; private set; }
        private BookingStatus bookingstatus;
        public BookingStatus BookingStatus
        {
            get
            {
                return GetBookingStatus();
            }
            private set
            {
                bookingstatus = value;
            }
        }


        public Booking()
        {
            throw new NotImplementedException("try using the static function Create ");
        }
        private Booking(Fuel_Policy Fuel_Policy,DateTime start,DateTime end,string VehicleId, Guid EmployeeId)
        {
            _fuelpolicy = Fuel_Policy;
            _tripwindow = MakeTripWindow(start ,end);
            Employee_id = EmployeeId;
             Vehicle_id = VehicleId;
            BookingStatus = BookingStatus.Scheduled;
        }
       
        public static Booking Create(Fuel_Policy Fuel_Policy, DateTime start, DateTime end, string Vehicle_Id,Guid EmployeeId)
        {
            var booking = new Booking(Fuel_Policy, start,end, Vehicle_Id, EmployeeId);
            booking.AddDomainEvent(new BookingForVehicleRequestEvent(Vehicle_Id));
            return booking;
        }
        public TripWindow MakeTripWindow(DateTime start,DateTime end)
        {
            return new TripWindow(start, end);
        }
        public BookingStatus GetBookingStatus()
        {

            if (bookingstatus != BookingStatus.Cancelled && _tripwindow.Start < DateTime.Now)
            {
              return BookingStatus.Cancelled;
            }
            return bookingstatus;


        }
        
        public void CancleBooking()
        {

            if(BookingStatus == BookingStatus.Active|| BookingStatus == BookingStatus.Completed)
                BookingStatus = BookingStatus.Cancelled;
             
        }
        public Booking StartTrip()
        {
            var Thestatus= GetBookingStatus();
            if (Thestatus != BookingStatus.Scheduled)
            {
                throw new Exception($"your trip is already {Thestatus} ");
            }
            if (_tripwindow.Start > DateTime.Now)
            {
                throw new Exception($" Your booking trip start after {_tripwindow.Start - DateTime.Now}");
            }
            BookingStatus = BookingStatus.Active;
            return this;
        }
        public Booking CompleteTrip(int meters)
        {
            var Thestatus= GetBookingStatus();
            if (Thestatus != BookingStatus.Active)
            {
                throw new Exception("the booking is not active ");
            }
            BookingStatus = BookingStatus.Completed;
            AddDomainEvent(new CompleteTheTripEvent(Vehicle_id, meters));
            return this;
        }

    }
}
