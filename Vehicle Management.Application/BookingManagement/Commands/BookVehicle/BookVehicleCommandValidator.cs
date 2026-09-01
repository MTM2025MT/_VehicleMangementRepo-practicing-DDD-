using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Application.BookingManagement.Commands.BookVehicle
{
    public class BookVehicleCommandCommandValidator:AbstractValidator<BookVehicleCommand>
    {
        public BookVehicleCommandCommandValidator()
        {
            RuleFor(Bv => Bv.EmployeeId)
                .NotEmpty()
                .WithMessage("the emplyee id is required");
            RuleFor(Bv => Bv.VehicleId)
                .NotEmpty()
                .WithMessage("the vehicle id is required");
            RuleFor(Bv => Bv.TimeEnd)
                .NotEmpty()
                .WithMessage("End date is required.")
                .GreaterThan(Bv => Bv.TimeStart) 
                .WithMessage("The end date must be after the start date.");
            RuleFor(Bv => Bv.Fuel_Policy)
                .NotEmpty();


        }
    }
}
