using FluentValidation.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;
using Vehicle_Management.Domain.Events;
using Vehicle_Management.Domain.IRepositories;
namespace Vehicle_Management.Application.EventsHandlers
{
    public  class BookingForVehicleRequestHandler:INotificationHandler<BookingForVehicleRequestEvent>
    {
        private IVehicleRepository _vehicleRepository;
         public BookingForVehicleRequestHandler(IVehicleRepository vehicleRepository) {

            _vehicleRepository=vehicleRepository;
          }
        public async Task Handle (BookingForVehicleRequestEvent request,CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.Vehicle_id);
            if (!vehicle.IsAvailable())
            {
                throw new Exception("the vehicle is not avaliable ");
            }
        }
    }
}
