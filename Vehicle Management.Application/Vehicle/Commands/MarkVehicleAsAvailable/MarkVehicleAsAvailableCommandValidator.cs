using FluentValidation;

namespace Vehicle_Management.Application.Vehicle.Commands.MarkVehicleAsAvailable
{
    // Validator for the MarkVehicleAsAvailableCommand
    public class MarkVehicleAsAvailableCommandValidator : AbstractValidator<MarkVehicleAsAvailableCommand>
    {
        public MarkVehicleAsAvailableCommandValidator()
        {
            RuleFor(x => x.VehicleId).NotEmpty().WithMessage("VehicleId must be provided.");
        }
    }
}