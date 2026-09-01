using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Domain.Aggregates.BookingAggregate
{
    public enum BookingStatus
    {
        Scheduled,
        Active,
        Completed,
        Cancelled
    }
}
