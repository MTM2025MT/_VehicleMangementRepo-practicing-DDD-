using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.SeedWork;
namespace Vehicle_Management.Domain.Aggregates.VehicleAggregate
{
    public class Vehicle : Entity<string>, IAggregateRoot
    {
        public string License_Plate { get; private set; }
        public Classification _classification { get; private set; }
        public Status _status { get; private set; }
        public int Odometer { get; private set; }
        public override string Id { get => License_Plate; protected set => base.Id = value; }

        public static Vehicle Create(string licensePlate, Classification classification, int odometer = 0)
        {
            var vehicle = new Vehicle
            {
                License_Plate = licensePlate,
                _classification = classification,
                _status = Status.Available,
                Odometer = odometer
            };

            return vehicle;
        }

        public void UpdateStatus(Status newStatus)
        {
            if (_status == Status.Maintenance && newStatus == Status.OutOnTrip)
            {
                throw new InvalidOperationException("Cannot start a trip while the vehicle is under maintenance.");
            }
            if (_status == Status.OutOnTrip && newStatus == Status.Maintenance)
            {
                throw new InvalidOperationException("Cannot mark a vehicle as maintenance while it is out on a trip.");
            }


            _status = newStatus;
        }
        public void MoveToMaintenance()
        {
            if (_status == Status.OutOnTrip)
            {
                throw new InvalidOperationException("Cannot move a vehicle to maintenance while it is out on a trip.");
            }
            _status = Status.Maintenance;

        }
        public void MoveToTrip()
        {
            if (_status == Status.Maintenance)
            {
                throw new InvalidOperationException("Cannot start a trip while the vehicle is under maintenance.");
            }
            _status = Status.OutOnTrip;
        }
        public void MarkAsAvilable()
        {
            _status = Status.Available;

        }
        public void UpdateOdometer(int newOdometer)
        {
            if (newOdometer < Odometer)
            {
                throw new InvalidOperationException("Ending mileage cannot be less than starting mileage.");
            }

            Odometer = newOdometer;
        }
        public void CompleteTheTrip(int meters)
        {
            MarkAsAvilable();
            UpdateOdometer(meters + Odometer);
        }
        public bool IsAvailable()
        {
            return _status == Status.Available;
        }
    }
}
