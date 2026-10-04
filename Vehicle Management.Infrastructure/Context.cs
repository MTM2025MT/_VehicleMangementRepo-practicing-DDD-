using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;
using Vehicle_Management.Domain.Aggregates.VehicleAggregate;
using Vehicle_Management.Domain.IRepositories;
using Vehicle_Management.Domain.SeedWork;

namespace Vehicle_Management.Infrastructure
{
    public class Context : DbContext, IUnitOfWork
    {
        IMediator mediator;
        public Context(DbContextOptions<Context> options, IMediator Meditor) : base(options)
        {
            mediator = Meditor;
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ClientRequest> ClientRequests { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DispatchDomainEventsAsync();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task DispatchDomainEventsAsync()
        {
            while (true)
            {
                var entitiesWithEvents = ChangeTracker.Entries<DomainEntity>()
                    .Select(e => e.Entity)
                    .Where(e => e.DomainEvents != null && e.DomainEvents.Any())
                    .ToList();
                if (!entitiesWithEvents.Any())
                {
                    break;
                }

                var events = entitiesWithEvents.SelectMany(e => e.DomainEvents).ToList();

                entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

                foreach (var domainEvent in events)
                {
                    await mediator.Publish(domainEvent);
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Booking entity
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);

                // Map TripWindow as an owned type (flattens to parent table)
                entity.OwnsOne(b => b._tripwindow, tripWindow =>
                {
                    tripWindow.Property(tw => tw.Start).HasColumnName("TripStart");
                    tripWindow.Property(tw => tw.End).HasColumnName("TripEnd");
                });
            });

            // Configure Vehicle entity
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.License_Plate)
                ;
                entity.HasIndex(v => v.License_Plate)
                 .IsUnique();


            });
        }
    }
}