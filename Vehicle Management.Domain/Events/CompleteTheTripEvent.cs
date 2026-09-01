using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Domain.Events
{
    public record CompleteTheTripEvent(string Vehicle_id,int meters) : INotification;

}
