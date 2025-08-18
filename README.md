# Task Manager API — README

A simple, secure **.NET 8 Web API** for task management with **JWT authentication**, **Entity Framework Core (code-first)**, **AutoMapper**, and **Swagger** for documentation.

---

## Table of Contents

- Overview
- Features
- Tech stack
- Project structure
- Getting started
- Configuration (env + appsettings)
- Database & Migrations
- Running the app
- Swagger
- API Endpoints (summary)
- Testing
- Recommended improvements
- Contribution
- License

---

## Overview

This API allows users to register, log in, and perform CRUD operations on tasks. Each user has private tasks that only they can access. The API uses DTOs for input/output, AutoMapper for mapping, and EF Core for persistence. Swagger (OpenAPI) is enabled for easy exploration.

---

## Features

- User registration and login (JWT tokens)
- Protected task endpoints (create, read, update, delete)
- DTOs + AutoMapper
- Entity Framework Core (code-first)
- Swagger/OpenAPI documentation
- Optional: Serilog logging and basic health checks (if configured)

---

## Tech stack

- .NET 8 (ASP.NET Core Web API)
- ASP.NET Core Identity
- JWT for authentication
- Entity Framework Core (SqlServer)
- AutoMapper
- Swagger (Swashbuckle)

---

## Project structure

```
TaskManager.API/
├─ Controllers/
├─ DTOs/
├─ Data/              # DbContext, Migrations, Seed
├─ Mappings/          # AutoMapper profiles
├─ Models/            # Entities
├─ Repositories/      # Business logic, interfaces
├─ Contracts/         # Interfaces for repositories
├─ Program.cs
├─ appsettings.json
```

---

## Getting started

### Prerequisites

- .NET 8 SDK installed
- SQL Server (or LocalDB)
- Optional: Docker (if using containerized DB)

### Quick start

1. Clone the repo:

```bash
git clone https://github.com/Adetola-Oluwaseyi/TaskManager.Api.git
cd TaskManager.Api
```

2. Configure connection string in `appsettings.Development.json` or use user-secrets/.env (see configuration section).

3. Apply migrations:

```bash
dotnet ef database update
```

4. Run the API:

```bash
dotnet run
```

5. Open Swagger UI (usually at `https://localhost:5001/swagger`) to explore endpoints.

---

## Configuration (env + appsettings)

Use `appsettings.json` for defaults and override with `appsettings.Development.json` or environment variables.

**Important environment variables / settings**:

- `ConnectionStrings:TaskManagerDbConnectionString` — SQL Server connection string
- `Jwt:Key` — secret signing key (store securely)
- `Jwt:Issuer`
- `Jwt:Audience`

**Security note:** Do not commit secrets to source control. Use `dotnet user-secrets`, environment variables, or Azure Key Vault in production.

---

## Database & Migrations

- Use EF Core Code-First migrations. Keep migrations small and descriptive.
- Use `dotnet ef migrations add <Name>` then `dotnet ef database update`.
- Include a small seed script to create an admin/test user and sample tasks for local/dev.

---

## Running the app

- Development: `dotnet run`
- Docker: add a `Dockerfile` and `docker-compose.yml` to run API + SQL Server locally.
- CI/CD: use GitHub Actions to run tests and publish container images.

---

## Swagger

Swagger is enabled. Make sure to protect non-development environments from exposing Swagger publicly. The generated OpenAPI spec is useful for Postman/clients.

---

## API Endpoints (summary)

**Authentication**

- `POST /api/v1/auth/register` — register user (Email, Password, FirstName, LastName)
- `POST /api/v1/auth/login` — login and receive JWT

**Tasks (JWT protected)**

- `POST /api/v1/tasks` — create a task
- `GET /api/v1/tasks` — list tasks (supports filters: status, due date, priority)
- `GET /api/v1/tasks/{id}` — get task by id (owner only)
- `PUT /api/v1/tasks/{id}` — update task
- `DELETE /api/v1/tasks/{id}` — delete task

---

## Testing

- Use xUnit for unit tests and integration tests.
- For integration tests, use the WebApplicationFactory and an in-memory or ephemeral SQL instance.
- Aim for tests for controller behavior, services, and repository logic.

---

## Recommended improvements (next steps)

- Add **refresh tokens** and revocation store
- Properly implement **role-based access** (admin vs user)
- Dockerize the app and SQL Server
- Add **logging** (Serilog + Seq)
- Add **health checks** and metrics (Prometheus)
- Add **unit & integration tests** coverage

---

## Contribution

1. Fork the repo
2. Create a branch: `feature/your-feature`
3. Make changes and add tests
4. Submit a PR with a clear description

---

## License

MIT

---
