using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Application.Vehicle.Queries.GetVehicleInfo
{
    public class GetVehicleInfoQueryValidator:AbstractValidator<GetVehicleInfoQuery>
    {
        public GetVehicleInfoQueryValidator()
        {
            RuleFor(Gvi => Gvi.Vehicle_Id)
                .NotEmpty();
        }
    }
}
