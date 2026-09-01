using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Events;
using Vehicle_Management.Domain.IRepositories;

namespace Vehicle_Management.Application.EventsHandlers
{
    public class CompleteTheTripEventHandler:INotificationHandler<CompleteTheTripEvent>
    {
        private IVehicleRepository _vehicleRepository;
        public CompleteTheTripEventHandler(IVehicleRepository vehicleRepository)
        {

            _vehicleRepository = vehicleRepository;
        }
        public async Task Handle(CompleteTheTripEvent request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.Vehicle_id);
            vehicle.CompleteTheTrip(request.meters);
            if (!vehicle.IsAvailable())
            {
                throw new Exception("completing by making the vehicle available didn't work ");
            }
        }
    }
}
