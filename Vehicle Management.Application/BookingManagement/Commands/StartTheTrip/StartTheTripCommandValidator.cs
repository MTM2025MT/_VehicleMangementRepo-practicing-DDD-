using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Application.BookingManagement.Commands.StartTheTrip;

namespace Vehicle_Management.Application.Booking.Commands.StartTheTrip
{
    public class StartTheTripCommandValidator:AbstractValidator<StartTheTripCommand>
    {
        public StartTheTripCommandValidator() {
         
            RuleFor(STT=>STT.Booking_Id).NotEmpty().WithMessage("the booking id is required ");
        
        }
    }
}
