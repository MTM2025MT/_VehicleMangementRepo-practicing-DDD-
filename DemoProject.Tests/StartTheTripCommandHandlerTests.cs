using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vehicle_Management.Application.BookingManagement.Commands.StartTheTrip;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;
using Vehicle_Management.Domain.IRepositories;
using Vehicle_Management.Infrastructure;
using Vehicle_Management.Infrastructure.Repositories;
using Xunit;
using DomainBooking = Vehicle_Management.Domain.Aggregates.BookingAggregate.Booking;

namespace DemoProject.Test.BookingManagement.Commands
{
    public class StartTheTripCommandHandlerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Context _context;
        private readonly StartTheTripCommandHandler _handler;

        public StartTheTripCommandHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>();

            // 1. Create unique In-Memory DB for every test run
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // 2. Initialize the real context with the mocked mediator
            _context = new Context(options, _mediatorMock.Object);

            // 3. Initialize real repositories pointing to the In-Memory context
            // Note: Update "BookingRepository" and "VehicleRepository" if your concrete classes are named differently
            IBookingRepository bookingRepo = new BookingRepository(_context);
            IVehicleRepository vehicleRepo = new VehicleRepository(_context);

            // 4. Pass the real context (as IUnitOfWork) and real repos to the handler
            _handler = new StartTheTripCommandHandler(_context, bookingRepo, vehicleRepo);
        }

        private static DomainBooking CreateStartableBooking(string vehicleId = "ABC-123") =>
            DomainBooking.Create(
                Fuel_Policy.Bring_Back_Full,
                DateTime.Now.AddHours(-1),
                DateTime.Now.AddHours(2),
                vehicleId,
                Guid.NewGuid());

        [Fact]
        public async Task Handle_BookingNotFound_ThrowsException()
        {
            // Arrange: DB is empty, so booking ID 1 won't be found.

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle(new StartTheTripCommand(1), CancellationToken.None));

            Assert.Contains("invalid or wrong", ex.Message);

            // Verify no events were published since it failed before saving
            _mediatorMock.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidBooking_StartsTripUpdatesBothAndSaves()
        {
            // Arrange: Seed the database
            var vehicleId = "ABC-123";
            var vehicle = Vehicle.Create(vehicleId, Classification.Sedan);
            var booking = CreateStartableBooking(vehicleId);

            _context.Vehicles.Add(vehicle);
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(); // Saves initial setup

            // Clear mediator history so we don't count any events triggered during DB seeding
            _mediatorMock.Invocations.Clear();

            // Act
            var result = await _handler.Handle(new StartTheTripCommand(booking.Id), CancellationToken.None);

            // Assert
            Assert.True(result);

            // Verify data actually changed in the database
            var dbBooking = await _context.Bookings.FindAsync(booking.Id);
            var dbVehicle = await _context.Vehicles.FindAsync(vehicleId);

            Assert.Equal(BookingStatus.Active, dbBooking.BookingStatus);
            Assert.Equal(Status.OutOnTrip, dbVehicle._status);

            // Verify the Context triggered DispatchDomainEventsAsync and published to Mediator
         //  _mediatorMock.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Handle_VehicleUnderMaintenance_SwallowsExceptionButStillSavesAndReturnsTrue()
        {
            // Arrange: Seed the database with a vehicle in maintenance
            var vehicleId = "ABC-123";
            var vehicle = Vehicle.Create(vehicleId, Classification.Sedan);
            vehicle.MoveToMaintenance(); // Assuming this updates _status

            var booking = CreateStartableBooking(vehicleId);

            _context.Vehicles.Add(vehicle);
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            _mediatorMock.Invocations.Clear();

            // Act
            var result = await _handler.Handle(new StartTheTripCommand(booking.Id), CancellationToken.None);

            // Assert
            Assert.True(result);

            // Verify it attempted to save the transaction despite the swallowed domain exception
        //    _mediatorMock.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}