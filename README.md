# MartenEventSourcing

A sample .NET Core project demonstrating **Event Sourcing** and **CQRS** patterns using [Marten](https://martendb.io/) on PostgreSQL.

---

## Overview

This project implements an event-sourced system leveraging Marten, a powerful document database and event store for .NET that sits on top of PostgreSQL.
The core idea behind event sourcing is to store state changes as a sequence of immutable events rather than just storing the current state. 
This approach provides a reliable audit log, enables temporal queries, and supports complex domain logic via event replay.

---

## Key Concepts

- **Event Sourcing**: Instead of persisting the current state, every change to an entity is stored as an event. The system state is rebuilt by replaying these events.
- **CQRS (Command Query Responsibility Segregation)**: Separation of read and write operations to optimize scalability and maintainability.
- **Domain-Driven Design (DDD)**: The code is structured to model the domain, emphasizing business logic and entities.
- **Marten**: Acts as both the document database and the event store, simplifying infrastructure by combining persistence and event sourcing in one tool.

---

## Technologies Used

- [.NET Core 6.0](https://dotnet.microsoft.com/en-us/)
- [Marten](https://martendb.io/) (Event sourcing and document database)
- [PostgreSQL](https://www.postgresql.org/) (Database)
- [xUnit](https://xunit.net/) (Unit testing)

---

### Prerequisites

- .NET Core SDK 6.0 or later
- PostgreSQL database (local or remote)
- Docker (optional, for running PostgreSQL easily)

