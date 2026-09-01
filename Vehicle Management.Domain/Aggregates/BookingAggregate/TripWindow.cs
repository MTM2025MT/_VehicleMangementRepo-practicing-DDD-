using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Domain.Aggregates.BookingAggregate
{
    public record TripWindow
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        internal TripWindow(DateTime start, DateTime end)
        {
            if (start >= end)
                throw new Exception("Start must be before End");

            Start = start;
            End = end;
        }
    }
}
