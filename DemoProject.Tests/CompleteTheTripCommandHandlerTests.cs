using Moq;
using Vehicle_Management.Application.BookingManagement.Commands.CompleteTheTrip;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;
using Vehicle_Management.Domain.IRepositories;
using Xunit;
using DomainBooking = Vehicle_Management.Domain.Aggregates.BookingAggregate.Booking;

namespace DemoProject.Test.BookingManagement.Commands
{
    public class CompleteTheTripCommandHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CompleteTheTripCommandHandler _handler;

        public CompleteTheTripCommandHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new CompleteTheTripCommandHandler(
                _bookingRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        private static DomainBooking CreateActiveBooking()
        {
            var booking = DomainBooking.Create(
                Fuel_Policy.Prepaid_Fuel,
                DateTime.Now.AddHours(-2),
                DateTime.Now.AddHours(2),
                "ABC-123",
                Guid.NewGuid());
            booking.StartTrip();
            return booking;
        }

        [Fact]
        public async Task Handle_BookingNotFound_ThrowsInvalidOperationException()
        {
            _bookingRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((DomainBooking)null);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(new CompleteTheTripCommand(1, 100), CancellationToken.None));
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ActiveBooking_CompletesTripUpdatesAndSaves()
        {
            var booking = CreateActiveBooking();
            _bookingRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(booking);
            _bookingRepositoryMock.Setup(r => r.UpdateAsync(booking)).ReturnsAsync(booking);

            var result = await _handler.Handle(new CompleteTheTripCommand(1, 150), CancellationToken.None);

            Assert.True(result);
            Assert.Equal(BookingStatus.Completed, booking.BookingStatus);
            _bookingRepositoryMock.Verify(r => r.UpdateAsync(booking), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_BookingNotActive_ThrowsWrappedException()
        {
            var booking = DomainBooking.Create(
                Fuel_Policy.Prepaid_Fuel,
                DateTime.Now.AddHours(1),
                DateTime.Now.AddHours(3),
                "ABC-123",
                Guid.NewGuid());
            _bookingRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(booking);

            var ex = await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle(new CompleteTheTripCommand(1, 100), CancellationToken.None));
            Assert.Contains("complete the trip", ex.Message);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}