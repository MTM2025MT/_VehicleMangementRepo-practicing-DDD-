using Moq;
using Vehicle_Management.Application.BookingManagement.Commands.BookVehicle;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;
using Vehicle_Management.Domain.IRepositories;
using Xunit;
using DomainBooking = Vehicle_Management.Domain.Aggregates.BookingAggregate.Booking;

namespace DemoProject.Test.BookingManagement.Commands
{
    public class BookVehicleCommandHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly BookVehicleCommandHandler _handler;

        public BookVehicleCommandHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new BookVehicleCommandHandler(
                _unitOfWorkMock.Object,
                _bookingRepositoryMock.Object,
                _vehicleRepositoryMock.Object);
        }

        private static BookVehicleCommand CreateValidCommand() =>
            new BookVehicleCommand(
                DateTime.Now.AddHours(1),
                DateTime.Now.AddHours(3),
                Guid.NewGuid(),
                "ABC-123",
                Fuel_Policy.Bring_Back_Full);

        [Fact]
        public async Task Handle_ValidCommand_AddsBookingAndSavesChanges()
        {
            var command = CreateValidCommand();
            _bookingRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<DomainBooking>()))
                .ReturnsAsync((DomainBooking b) => b);

            var result = await _handler.Handle(command, CancellationToken.None);

            _bookingRepositoryMock.Verify(r => r.AddAsync(It.IsAny<DomainBooking>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesBookingWithCorrectData()
        {
            var command = CreateValidCommand();
            DomainBooking captured = null;
            _bookingRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<DomainBooking>()))
                .Callback<DomainBooking>(b => captured = b)
                .ReturnsAsync((DomainBooking b) => b);

            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(captured);
            Assert.Equal(command.VehicleId, captured.Vehicle_id);
            Assert.Equal(command.EmployeeId, captured.Employee_id);
        }

        [Fact]
        public async Task Handle_RepositoryThrows_WrapsException()
        {
            var command = CreateValidCommand();
            _bookingRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<DomainBooking>()))
                .ThrowsAsync(new InvalidOperationException("db failure"));

            var ex = await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("db failure", ex.Message);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle2_VehicleNotFound_ThrowsException()
        {
            var command = CreateValidCommand();
            _vehicleRepositoryMock
                .Setup(r => r.GetByIdAsync(command.VehicleId))
                .ReturnsAsync((Vehicle)null);

            var ex = await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle2(command, CancellationToken.None));
            Assert.Contains("no vicle", ex.Message);
        }

        [Fact]
        public async Task Handle2_VehicleNotAvailable_ThrowsException()
        {
            var command = CreateValidCommand();
            var vehicle = Vehicle.Create("ABC-123", Classification.Sedan);
            vehicle.MoveToTrip();
            _vehicleRepositoryMock
                .Setup(r => r.GetByIdAsync(command.VehicleId))
                .ReturnsAsync(vehicle);

            var ex = await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle2(command, CancellationToken.None));
            Assert.Contains("not avilable", ex.Message);
        }

        [Fact]
        public async Task Handle2_VehicleAvailable_AddsBooking()
        {
            var command = CreateValidCommand();
            var vehicle = Vehicle.Create("ABC-123", Classification.Sedan);
            _vehicleRepositoryMock
                .Setup(r => r.GetByIdAsync(command.VehicleId))
                .ReturnsAsync(vehicle);
            _bookingRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<DomainBooking>()))
                .ReturnsAsync((DomainBooking b) => b);

            var result = await _handler.Handle2(command, CancellationToken.None);

            _bookingRepositoryMock.Verify(r => r.AddAsync(It.IsAny<DomainBooking>()), Times.Once);
            Assert.Equal(0, result);
        }
    }
}