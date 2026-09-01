using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Application.BookingManagement.Commands.CompleteTheTrip
{
    public record CompleteTheTripCommand(int Booking_Id,int meters) :IRequest<bool>;
}
