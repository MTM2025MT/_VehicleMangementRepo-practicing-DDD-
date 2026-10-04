using Moq;
using Vehicle_Management.Application.BookingManagement.Commands.CancelBooking;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;
using Vehicle_Management.Domain.IRepositories;
using Xunit;
using DomainBooking = Vehicle_Management.Domain.Aggregates.BookingAggregate.Booking;

namespace DemoProject.Test.BookingManagement.Commands
{
    public class CancelBookingCommandHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CancelBookingCommandHandler _handler;

        public CancelBookingCommandHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new CancelBookingCommandHandler(
                _bookingRepositoryMock.Object,
                _vehicleRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        private static DomainBooking CreateActiveBooking()
        {
            var booking = DomainBooking.Create(
                Fuel_Policy.Bring_Back_Full,
                DateTime.Now.AddHours(-2),
                DateTime.Now.AddHours(2),
                "ABC-123",
                Guid.NewGuid());
            return booking;
        }

        [Fact]
        public async Task Handle_BookingNotFound_ThrowsException()
        {
            _bookingRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((DomainBooking)null);

            var ex = await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle(new CancelBookingCommand(1), CancellationToken.None));
            Assert.Contains("not valid", ex.Message);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ActiveBooking_CancelsAndReturnsTrue()
        {
            var booking = CreateActiveBooking();
            _bookingRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(booking);
            _bookingRepositoryMock.Setup(r => r.UpdateAsync(booking)).ReturnsAsync(booking);

            var result = await _handler.Handle(new CancelBookingCommand(1), CancellationToken.None);

            Assert.True(result);
            Assert.Equal(BookingStatus.Cancelled, booking.BookingStatus);
            _bookingRepositoryMock.Verify(r => r.UpdateAsync(booking), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_UpdateReturnsNull_ReturnsFalseAndDoesNotSave()
        {
            var booking = CreateActiveBooking();
            _bookingRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(booking);
            _bookingRepositoryMock.Setup(r => r.UpdateAsync(booking)).ReturnsAsync((DomainBooking)null);

            var result = await _handler.Handle(new CancelBookingCommand(1), CancellationToken.None);

            Assert.False(result);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}