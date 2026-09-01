using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vehicle_Management.Application.Vehicle.Queries.GetAllVehiclesInfo;
using Vehicle_Management.Application.Vehicle.Queries.GetVehicleInfo;
using Vehicle_Management.Application.Vehicle.Commands.SendToMaintenance;
using Vehicle_Management.Application.Vehicle.Commands.MarkVehicleAsAvailable;
using Vehicle_Management.Application.Vehicle.Commands._ِAddVehicle;

namespace Vehicle_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IMediator meditar;

        public VehicleController(IMediator Mediator)
        {
            meditar = Mediator;
        }


        [HttpGet("vehicles")]
        public async Task<IActionResult> GetAllVehicles()
        {
            try
            {
                var result = await meditar.Send(new Vehicle_Management.Application.Vehicle.Queries.GetAllVehiclesInfo.GetAllVehiclesInfoQuery());
                if (result is not null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpGet("vehicle")]
        public async Task<IActionResult> GetVehicleById(GetVehicleInfoQuery query)
        {
            try
            {
                var result = await meditar.Send(query);
                if (result is not null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpPost("vehicles")]
        public async Task<IActionResult> AddVehicle([FromBody] AddVehicleCommand? command)
        {
            try
            {
                if (command is null)
                    return BadRequest("AddVehicleCommand cannot be null.");

                var result = await meditar.Send(command);


                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpPost("vehicles/{id}/maintenance")]
        public async Task<IActionResult> SendToMaintenance([FromRoute] string id, [FromBody]SendToMaintenaceCommand? command)
        {
            try
            {
                if (command is null)
                    command = new SendToMaintenaceCommand(id);


                var result = await meditar.Send(command);


                    return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpPost("vehicles/{id}/available")]
        public async Task<IActionResult> MarkAsAvailable([FromRoute] string id, [FromBody] MarkVehicleAsAvailableCommand? command)
        {
            try
            {
                if (command is null)
                    command = new MarkVehicleAsAvailableCommand(id);

                var result = await meditar.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }
        }
        
    }
}
