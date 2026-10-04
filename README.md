# Vehicle Management

A .NET 8 DDD-style vehicle management system built around clear domain boundaries, domain events, and MediatR-based application workflows.

## Project Idea

This system manages vehicle booking and trip lifecycle operations.

The main flow is:

1. A user creates a booking request.
2. The `Booking` aggregate is created in the Domain layer.
3. The aggregate raises a domain event.
4. A MediatR handler in the Application layer reacts to the event.
5. The handler coordinates persistence and related business actions.

## Main Aggregates

### `Booking` Aggregate
Represents the booking lifecycle for a vehicle trip.

Responsibilities:
- create a booking
- track booking window
- track booking status
- start a trip
- complete a trip
- cancel a booking

### `Vehicle` Aggregate
Represents the vehicle inventory and vehicle-related rules.

Responsibilities:
- vehicle state
- vehicle availability
- vehicle-related booking interactions

## Domain Events Flow

The system uses domain events to keep the Domain layer isolated from application concerns.

### Example Flow
- `Booking.Create(...)`
  - creates a new `Booking`
  - raises `BookingForVehicleRequestEvent`

- `Booking.StartTrip()`
  - validates the current booking state
  - changes status to `Active`

- `Booking.CompleteTrip(...)`
  - validates the booking is active
  - changes status to `Completed`
  - raises `CompleteTheTripEvent`

### Why this matters
- domain rules stay inside the aggregate
- handlers stay in the Application layer
- infrastructure concerns stay outside the domain model

## Solution Layers

- `Vehicle_Management.Domain`
  - aggregates
  - domain events
  - domain rules

- `Vehicle_Management.Application`
  - MediatR commands, queries, handlers
  - validation
  - orchestration

- `Vehicle_Management.Infrastructure`
  - EF Core context
  - repositories
  - persistence

- `Vehicle_Management`
  - ASP.NET Core API
  - DI configuration
  - authentication
  - controllers

## Notes

- Domain events are dispatched through MediatR.
- Handlers must be registered from the Application assembly.
- EF Core needs a private parameterless constructor for aggregate materialization.

## Getting Started

1. Configure the connection string.
2. Configure Azure AD authentication if enabled.
3. Run the API project.
4. Use Swagger or your client to test booking flows.
