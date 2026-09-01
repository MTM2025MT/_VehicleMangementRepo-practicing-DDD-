using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Application.BookingManagement.Commands.CompleteTheTrip
{
    public class CompleteTheTripCommandValidator: AbstractValidator<CompleteTheTripCommand>
    {
        public CompleteTheTripCommandValidator()
        {
            RuleFor(CTT => CTT.Booking_Id)
                .NotEmpty()
                .WithMessage("the booking id required");
        }

    }
}
