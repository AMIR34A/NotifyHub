# NotifyHub

A multi-channel notification service built with **.NET**, **Clean Architecture**, and **Domain-Driven Design** as a self-study project to practice backend patterns and distributed systems concepts.

## Overview

NotifyHub accepts notification requests and routes them to the appropriate provider — **SMS**, **Email**, or **Telegram Bot** — based on the notification type. It uses a customized Outbox Pattern to persist notifications before dispatching them, ensuring reliability even under failure conditions.

---

## Architecture

The solution follows **Clean Architecture** with four numbered layers that enforce a strict dependency rule (outer layers depend on inner layers, never the reverse):

```
src/
├── 1.Core/
│   ├── NotifyHub.Core.Domain            # Entities, value objects, domain logic
│   ├── NotifyHub.Core.Contracts         # Interfaces and abstractions
│   ├── NotifyHub.Core.ApplicationService # CQRS handlers, application logic
│   └── NotifyHub.Core.BuildingBlocks    # Shared base types, Result pattern, base classes
│
├── 2.Infrastructure/
│   ├── NotifyHub.Infrastructure.Services          # SMS, Email, Telegram providers; RabbitMQ service
│   └── Data/
│       └── NotifyHub.Infrastructure.Data.SqlServer # Outbox persistence, repositories
│
├── 3.Endpoint/
│   └── NotifyHub.Endpoint.WebAPI        # ASP.NET Core Web API entry point
│
└── 4.Shared/
    └── NotifyHub.Shared.Utility         # Cross-cutting helpers and utilities
```

---

## Key Design Decisions

### Factory Pattern for Notification Routing
A factory is used to resolve the correct notification provider at runtime based on the notification type, keeping the dispatch logic decoupled from provider implementations.

### Customized Outbox Pattern
Incoming notifications are saved to the database before being dispatched. A background process reads the outbox and publishes messages, providing a basic at-least-once delivery guarantee and decoupling the API from external provider availability.

### Result Pattern
SMS and Email services return a `Result<T>` type instead of throwing exceptions for expected failures. This makes success and error paths explicit in the application layer.

### CQRS with MediatR
Commands and queries are separated using **MediatR**. Write operations (sending a notification) and read operations are handled by distinct handlers, keeping responsibilities clear.

### Custom RabbitMQ Integration
A custom `IMessageBusService` wraps RabbitMQ.Client v7's async API. A reusable consumer base class (`MessageBusConsumer<TMessage>`) is used for implementing background consumers, making it straightforward to add new message types.

### Resilience with Polly
The SMS service uses **Polly** retry policies to handle transient failures when calling external SMS providers.

---

## Tech Stack

| Concern | Technology |
|---|---|
| Language / Runtime | C# / .NET |
| Web API | ASP.NET Core |
| CQRS / Mediator | MediatR |
| Message Broker | RabbitMQ (RabbitMQ.Client v7) |
| Database | SQL Server (Outbox), MongoDB |
| Resilience | Polly |
| Logging | Serilog |
| Architecture | Clean Architecture + DDD |

---

## Notification Channels

- **Email** — via a configurable email provider
- **SMS** — via an external SMS gateway, with Polly retry on transient errors
- **Telegram Bot** — via the Telegram Bot API

---

## Purpose

This project was built as a hands-on learning exercise to apply and understand:

- Clean Architecture and DDD in a real codebase
- The Outbox Pattern for reliable message publishing
- CQRS with MediatR
- RabbitMQ integration in .NET with the v7 async client
- Resilience patterns with Polly
- Structured logging with Serilog

It is not a production-ready system, but a personal study project exploring how these patterns fit together.

---

## License

[MIT](LICENSE.txt)
