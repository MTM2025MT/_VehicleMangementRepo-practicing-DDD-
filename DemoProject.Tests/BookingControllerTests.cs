using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Vehicle_Management.Application.BookingManagement.Commands.BookVehicle;
using Vehicle_Management.Application.BookingManagement.Commands.CancelBooking;
using Vehicle_Management.Application.BookingManagement.Commands.CompleteTheTrip;
using Vehicle_Management.Application.BookingManagement.Commands.StartTheTrip;
using Vehicle_Management.Application.BookingManagement.Queries.GetBookingInfo;
using Vehicle_Management.Controllers;
using Vehicle_Management.Dtos;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;


namespace Vehicle_Management.Tests.Controllers
{
    public class BookingControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;

        public BookingControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
        }

        private BookingController CreateController() => new BookingController(_mediatorMock.Object);

        [Fact]
        public async Task BookVehicle_ReturnsOk_WithBookingId()
        {
            // Arrange
            var start = DateTime.UtcNow;
            var end = start.AddHours(2);

            var dto = new BookVehicleDto(
                DateOnly.FromDateTime(start),
                TimeOnly.FromDateTime(start),
                DateOnly.FromDateTime(end),
                TimeOnly.FromDateTime(end),
                Guid.NewGuid(),
                "V123",
                Fuel_Policy.Bring_Back_Full
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<BookVehicleCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(42);

            var controller = CreateController();

            // Act
            var result = await controller.BookVehicle(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(42, ok.Value);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        }

        [Fact]
        public async Task BookVehicle_WhenMediatorThrows_Returns500WithMessage()
        {
            // Arrange
            var start = DateTime.UtcNow;
            var end = start.AddHours(2);

            var dto = new BookVehicleDto(
                DateOnly.FromDateTime(start),
                TimeOnly.FromDateTime(start),
                DateOnly.FromDateTime(end),
                TimeOnly.FromDateTime(end),
                Guid.NewGuid(),
                "V123",
                Fuel_Policy.Bring_Back_Full
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<BookVehicleCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("boom"));

            var controller = CreateController();

            // Act
            var result = await controller.BookVehicle(dto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            var value = Assert.IsType<string>(objectResult.Value);
            Assert.Contains("boom", value);
        }

        [Fact]
        public async Task StartTheTrip_ReturnsOk_WhenMediatorReturnsTrue()
        {
            // Arrange
            var command = new StartTheTripCommand(1);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<StartTheTripCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var controller = CreateController();

            // Act
            var result = await controller.StartTheTrip(command);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, ok.Value);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        }

        [Fact]
        public async Task StartTheTrip_Returns400_WhenMediatorReturnsFalse()
        {
            // Arrange
            var command = new StartTheTripCommand(1);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<StartTheTripCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var controller = CreateController();

            // Act
            var result = await controller.StartTheTrip(command);

            // Assert
            var status = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, status.StatusCode);
        }

        [Fact]
        public async Task StartTheTrip_WhenMediatorThrows_Returns400WithException()
        {
            // Arrange
            var command = new StartTheTripCommand(1);
            var ex = new InvalidOperationException("bad");
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<StartTheTripCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(ex);

            var controller = CreateController();

            // Act
            var result = await controller.StartTheTrip(command);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Same(ex, objectResult.Value);
        }

        [Fact]
        public async Task CompleteTheTrip_ReturnsOk_WhenMediatorReturnsTrue()
        {
            // Arrange
            var command = new CompleteTheTripCommand(1, 100);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CompleteTheTripCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var controller = CreateController();

            // Act
            var result = await controller.CompleteTheTrip(command);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, ok.Value);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        }

        [Fact]
        public async Task CompleteTheTrip_Returns400_WhenMediatorReturnsFalse()
        {
            // Arrange
            var command = new CompleteTheTripCommand(1, 100);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CompleteTheTripCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var controller = CreateController();

            // Act
            var result = await controller.CompleteTheTrip(command);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task CompleteTheTrip_WhenMediatorThrows_Returns400WithException()
        {
            // Arrange
            var command = new CompleteTheTripCommand(1, 100);
            var ex = new Exception("err");
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CompleteTheTripCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(ex);

            var controller = CreateController();

            // Act
            var result = await controller.CompleteTheTrip(command);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Same(ex, objectResult.Value);
        }

        [Fact]
        public async Task CancelTheTrip_ReturnsOk_WhenMediatorReturnsTrue()
        {
            // Arrange
            var command = new CancelBookingCommand(5);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CancelBookingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var controller = CreateController();

            // Act
            var result = await controller.CancelTheTrip(command);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, ok.Value);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        }

        [Fact]
        public async Task CancelTheTrip_ReturnsProblem_WhenMediatorReturnsFalse()
        {
            // Arrange
            var command = new CancelBookingCommand(5);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CancelBookingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var controller = CreateController();

            // Act
            var result = await controller.CancelTheTrip(command);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            // Problem(...) sets the provided status code (500)
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            var pd = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal("Cancelation of Trip  Error", pd.Title);
            Assert.Equal("Failed to cancel the trip.", pd.Detail);
        }

        [Fact]
        public async Task CancelTheTrip_WhenMediatorThrows_Returns400WithException()
        {
            // Arrange
            var command = new CancelBookingCommand(5);
            var ex = new Exception("boom");
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CancelBookingCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(ex);

            var controller = CreateController();

            // Act
            var result = await controller.CancelTheTrip(command);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Same(ex, objectResult.Value);
        }

        [Fact]
        public async Task GetBookingById_ReturnsOk_WhenMediatorReturnsDto()
        {
            // Arrange
            var dto = new BookingInfoDto(
                Guid.NewGuid(),
                "V1",
                "Booked",
                "Bring_Back_Full",
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                "ModelX");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBookingInfoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            var controller = CreateController();

            // Act
            var result = await controller.GetBookingById(new GetBookingInfoQuery(1));

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(dto, ok.Value);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        }

        [Fact]
        public async Task GetBookingById_ReturnsNotFound_WhenMediatorReturnsNull()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBookingInfoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BookingInfoDto?)null);

            var controller = CreateController();

            // Act
            var result = await controller.GetBookingById(new GetBookingInfoQuery(1));

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetBookingById_WhenMediatorThrows_Returns400WithException()
        {
            // Arrange
            var ex = new Exception("fail");
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBookingInfoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(ex);

            var controller = CreateController();

            // Act
            var result = await controller.GetBookingById(new GetBookingInfoQuery(1));

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Same(ex, objectResult.Value);
        }
    }
}
