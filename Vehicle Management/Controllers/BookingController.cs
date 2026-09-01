using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vehicle_Management.Application.BookingManagement.Commands.BookVehicle;
using Vehicle_Management.Application.BookingManagement.Commands.CancelBooking;
using Vehicle_Management.Application.BookingManagement.Commands.CompleteTheTrip;
using Vehicle_Management.Application.BookingManagement.Commands.StartTheTrip;
using Vehicle_Management.Application.BookingManagement.Queries.GetBookingInfo;

namespace Vehicle_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private IMediator meditar;
        public BookingController(IMediator Mediator)
        {
            meditar = Mediator;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookVehicle(BookVehicleCommand command)
        {
            try
            {
                var booking_id = await meditar.Send(command);
                return Ok(booking_id);

            }
            catch (Exception ex)
            {
                return StatusCode(500,$"something wend wrong {ex}");

            }



        }

        [HttpPost("start")]
        public async Task<IActionResult> StartTheTrip(StartTheTripCommand command)
        {
            try
            {
                var result = await meditar.Send(command);
                if (result == true)
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                // preserve previous behavior of returning error details
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteTheTrip(CompleteTheTripCommand command)
        {
            try
            {
                var result = await meditar.Send(command);
                if (result == true)
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelTheTrip(CancelBookingCommand command)
        {
            try
            {
                var result = await meditar.Send(command);
                if (result == true)
                    return Ok(result);
                else (result == false)
                      return Problem(detail: "Failed to cancel the trip.", title: "Cancelation of Trip  Error", statusCode: StatusCodes.Status500InternalServerError);
  
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpGet("booking")]
        public async Task<IActionResult> GetBookingById(GetBookingInfoQuery query)
        {
            try
            {
                var result = await meditar.Send(query);
                 if(result is not null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }
        }
        /*
│
└── GET /bookings*/
    }
}
