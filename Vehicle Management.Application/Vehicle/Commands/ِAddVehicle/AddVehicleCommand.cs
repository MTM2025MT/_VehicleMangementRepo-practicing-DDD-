using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;

namespace Vehicle_Management.Application.Vehicle.Commands._ِAddVehicle
{
    public record AddVehicleCommand(string LicensePlate, Classification Classification, int Odometer = 0) : IRequest<MediatR.Unit>;
}
