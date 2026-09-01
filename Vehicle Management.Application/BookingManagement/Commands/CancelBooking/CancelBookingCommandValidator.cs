using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Application.BookingManagement.Commands.CancelBooking;

namespace Vehicle_Management.Application.BookingManagement.Commands.BookVehicle
{
    public class CancelBookingCommandValidator:AbstractValidator<CancelBookingCommand>
    {
        public CancelBookingCommandValidator()
        {
            RuleFor(Cb=>Cb.Booking_Id).NotEmpty().WithMessage("the booking id is requried");
        }

    }
}
