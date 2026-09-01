using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Application.Vehicle.Queries.GetAllVehiclesInfo
{
    /*
             public  string License_Plate { get; private set; }
        public Classification _classification { get; private set; }
        public Status _status { get; private set; }
        public int Odometer { get; private set; }
     */
    public record VehicleDto(string Id, string Class, string Status , int TakenMeters);
    public record GetAllVehiclesInfoQuery():IRequest<List<VehicleDto>>;

}
