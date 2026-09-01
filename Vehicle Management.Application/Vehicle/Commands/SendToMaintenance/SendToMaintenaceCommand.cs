using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Application.Vehicle.Commands.SendToMaintenance
{

    public record SendToMaintenaceCommand(string Vehicle_id) : IRequest<bool>;
}
