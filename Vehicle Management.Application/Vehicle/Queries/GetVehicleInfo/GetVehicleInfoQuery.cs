using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;

namespace Vehicle_Management.Application.Vehicle.Queries.GetVehicleInfo
{

    public record VehicleInfoDto(
    string Id,
    Classification Model,
    bool IsAvailable,
    int Odometer
);
    public record GetVehicleInfoQuery(string  Vehicle_Id): IRequest<VehicleInfoDto>;
}
