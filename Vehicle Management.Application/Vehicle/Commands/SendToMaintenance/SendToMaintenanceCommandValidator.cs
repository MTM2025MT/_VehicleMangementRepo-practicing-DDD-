using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Application.Vehicle.Commands.SendToMaintenance
{

    public class SendToMaintenanceCommandValidator:AbstractValidator<SendToMaintenaceCommand>
    {
        public SendToMaintenanceCommandValidator() {

            RuleFor(stmc => stmc.Vehicle_id).NotEmpty();
        }
    }
}
