# VisitTracking Backend (.NET Core)

## Project Overview
This backend powers a **Live Visit Tracking System**. It provides REST APIs for managing business entities (e.g., Department, Designation, Employees, Visits, Attachments, Follow-ups, etc.) and supports **JWT-based authentication** for secure access.

**Technologies used**
- **.NET (ASP.NET Core Web API)**
- **Entity Framework Core**
- **SQL Server** (configured via connection string)
- **JWT Authentication**
- **Swagger / OpenAPI**
- **Repository Pattern**
- DTO-based request/response contracts (clean separation of API and business logic)

### High-level Architecture
```text
Client
  → HTTP Request
    → Controller (API Layer)
      → Service Layer (Business Logic + Validation orchestration)
        → Repository Layer (Data Access)
          → DbContext (EF Core)
            → SQL Database
      ← Response DTO
```

---

## Folder Structure (Important Parts)

> Note: Your solution contains multiple projects. The names below reflect the typical usage in this codebase.

### `Controllers/`
- Contains API endpoints.
- Receives HTTP requests and returns HTTP responses.
- Delegates business logic to services.

### `Services/`
- Implements business logic (create/update/delete rules, mapping to domain models, audit logging).
- Uses repositories for database operations.
- Performs authorization checks and prepares values for persistence.

### `Repository/Repositories/`
- Data access layer using **Entity Framework Core**.
- Executes queries/commands against the database.
- Typically uses `DbSet<TEntity>` and includes navigation properties where needed.

### `DTOs/`
- Data Transfer Objects used for API inputs and outputs.
- Keeps API contracts stable and prevents direct EF entity exposure.

### `Models / Entities`
- EF Core entity classes (e.g., `Department`, `Auditlog`, `Visit`, etc.).
- Encapsulate database structure and relationships.

### `Data / DbContext`
- EF Core `AppDbContext` configures entity mappings and database sets.
- The DbContext is the bridge between domain entities and SQL Server.

### `Middleware`
- Exception handling / request pipeline configuration (Global handling if present).
- Authentication/Authorization middleware is part of the ASP.NET Core pipeline.

### `Helpers/`
- Utility classes used by services (e.g., templates, password utilities).

### `Interfaces/`
- Interfaces for services and repositories (promotes testability and separation of concerns).

### `Migrations`
- EF Core migrations track schema changes.
- Generated from DbContext + entity changes.

---

## API Request Flow (Step-by-Step)

```text
Client Request
  → Controller
    → Service Layer
      → Repository Layer
        → DbContext
          → SQL Database
            → Entity results
      ← Service returns DTOs
    ← Controller returns ActionResult / status code
← Client Response
```

### Where things happen
- **Validation**
  - Request model validation occurs via `[ApiController]` + model validation.
  - Some additional checks may exist inside services/repositories.
- **Business logic**
  - Implemented in the **Service layer** (e.g., mapping DTO → entity, audit log creation).
- **Database queries**
  - Implemented inside the **Repository layer**.
- **DTO mapping**
  - Typically happens in service methods (entity → response DTO; request DTO → entity).

---

## Authentication Flow (JWT)

### 1) Login API
- Client calls login endpoint.
- Server validates credentials.
- Server generates a **JWT token**.

### 2) JWT generation
- JWT token includes claims (notably `id`, and role claims depending on your app).
- Token is returned to the client.

### 3) Authorization middleware
- On subsequent requests, the token is sent in:
  - `Authorization: Bearer <token>`
- ASP.NET Core validates:
  - issuer, audience, signature, expiration
- Protected endpoints require an authenticated user.

### 4) Protected APIs
- Controllers are protected using `[Authorize]` / authorization configuration.
- Services can access user claims (e.g., user id) using `HttpContext.User`.

---

## Database Flow (EF Core + SQL Server)

### How EF Core works
- **Entities** define the shape of your database tables.
- **DbContext** exposes `DbSet<TEntity>` and manages queries/updates.
- EF Core translates LINQ queries into SQL.

### Migrations
- Migrations are generated from model changes.
- Apply migrations to keep schema in sync.

### Relationships & Foreign Keys
- Navigation properties (e.g., Department ↔ Designation) define relational mapping.
- `Include(...)` in repositories loads related data where needed.

---

## API Module Flows

### Department Module
```text
Request
  → DepartmentController
    → DepartmentService
      → DepartmentRepository
        → DbContext
          → SQL Database
      ← Department entity
    → DepartmentService maps to DepartmentResponseDto
  ← Controller returns JSON response
```

### Designation Module
```text
Request
  → DesignationController
    → DesignationService
      → DesignationRepository
        → DbContext
          → SQL Database
      ← Designation entity
  ← Controller returns JSON response
```

> Each module follows the same layered pattern:
**Controller → Service → Repository → DbContext → SQL**

---

## Setup Instructions

1. **Clone the repository**
```bash
git clone <your-repo-url>
cd VisitTrackingBackend
```

2. **Update connection string**
- Edit `appsettings.json` (and/or environment-specific files):
  - `ConnectionStrings:DefaultConnection`

3. **Run migrations (if needed)**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. **Run the API**
```bash
dotnet run --project VisitTracking.Api
```

5. **Swagger**
- Open:
  - `http://localhost:<port>/swagger`
- Or:
  - `http://localhost:<port>/swagger/index.html`

---

## Environment Variables / appsettings values

At minimum, ensure you have:

- **Connection string**
  - `ConnectionStrings:DefaultConnection`

- **JWT configuration**
  - `Jwt:Key`
  - `Jwt:Issuer`
  - `Jwt:Audience`

> Logging can also be configured in `appsettings.json`.

---

## Error Handling

### Validation responses
- Model validation errors are handled automatically by ASP.NET Core (thanks to `[ApiController]`).

### Exception handling
- When unhandled exceptions occur, the API should return a proper error response.
- If you have global exception middleware, it formats errors consistently.

### Status codes (typical)
- `200 OK` – Successful GET/PUT operations
- `201 Created` – Successful POST operations
- `400 BadRequest` – Validation errors / malformed inputs
- `401 Unauthorized` – Missing or invalid JWT
- `404 NotFound` – Resource not found
- `500 InternalServerError` – Unexpected server error

---

## Best Practices Used

- **Dependency Injection** (`builder.Services.AddScoped...`)
- **Async/Await** for database + I/O
- **Repository Pattern** for data access separation
- **DTO usage** for API contracts
- **Clean layering** between API, services, and repositories
- **Swagger** for developer-friendly API exploration

---

## Future Improvements (Recommended)

You can extend the system safely with:
- **Role-based authorization** improvements (fine-grained policies)
- **JWT refresh tokens** / token rotation
- **Azure Blob Storage** for scalable file storage (instead of local `Uploads/`)
- **Face verification** integration for enhanced employee attendance security
- **Geo-fencing** logic in visit validation
- **Employee attendance system** module to record check-in/out
- **Global error middleware** and structured logging (Serilog sinks)

---

## Architecture Diagram (Text Arrows)

```text
HTTP Client
  -> Controllers
    -> Services
      -> Repositories
        -> DbContext
          -> SQL Server
            <- Results
    <- DTOs/Responses
  <- HTTP Response
