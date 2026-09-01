using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Application.BookingManagement.Commands.StartTheTrip
{
    public record StartTheTripCommand(int Booking_Id) : IRequest<bool>;
}
