using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Application.BookingManagement.Queries.GetBookingInfo
{
    public class GetBookingInfoQueryValidator:AbstractValidator<GetBookingInfoQuery>
    {

        public GetBookingInfoQueryValidator() {
          RuleFor(Gbi=>Gbi.id).NotEmpty();
        }
    }
}
