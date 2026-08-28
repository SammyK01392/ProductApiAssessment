# Product API Assessment

## Overview

A RESTful Backend API built using **ASP.NET Core .NET 8** for managing Products and Items.

The solution is designed with scalability, maintainability, separation of concerns, security, and testability in mind.

### Key Features

* Product CRUD operations
* Item management associated with Products
* JWT authentication
* Refresh token support
* API versioning
* FluentValidation
* Repository and Unit of Work patterns
* Global exception handling middleware
* Structured logging with Serilog
* Swagger/OpenAPI documentation
* Pagination for collection endpoints
* Entity Framework Core with SQL Server
* Docker and Docker Compose support
* Unit and integration testing with xUnit, Moq, and WebApplicationFactory

---

## Architecture

The solution follows a layered architecture:

```text
Client
  |
  v
ASP.NET Core Web API
  |
  v
Application Layer
  |
  v
Domain Layer
  |
  v
Infrastructure Layer
  |
  v
SQL Server
```

### Layers

* **API** – Controllers, middleware, filters, API configuration, Swagger
* **Application** – DTOs, services, interfaces, validators, mapping
* **Domain** – Entities, enums, domain events, domain exceptions
* **Infrastructure** – EF Core, repositories, Unit of Work, database configuration, JWT services

This separation keeps business logic independent from API and database concerns.

---

## Tech Stack

| Technology            | Purpose                       |
| --------------------- | ----------------------------- |
| .NET 8                | Application framework         |
| C#                    | Programming language          |
| ASP.NET Core Web API  | REST API                      |
| Entity Framework Core | ORM / Data Access             |
| SQL Server            | Database                      |
| JWT                   | Authentication                |
| FluentValidation      | Request validation            |
| AutoMapper            | Object mapping                |
| Serilog               | Structured logging            |
| Swagger / OpenAPI     | API documentation             |
| xUnit                 | Testing                       |
| Moq                   | Mocking                       |
| WebApplicationFactory | API integration testing       |
| Docker                | Containerization              |
| Docker Compose        | Multi-container orchestration |

---

## Project Structure

```text
ProductApiAssessment/
│
├── src/
│   ├── Application/
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Mapping/
│   │   ├── Services/
│   │   └── Validators/
│   │
│   ├── Domain/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   ├── Events/
│   │   └── Exceptions/
│   │
│   ├── Infrastructure/
│   │   └── Data/
│   │       ├── Configurations/
│   │       ├── Migrations/
│   │       ├── Repository/
│   │       ├── Identity/
│   │       ├── ApplicationDbContext.cs
│   │       └── UnitOfWork.cs
│   │
│   └── ProductApiAssessment/
│       ├── Controllers/
│       ├── Extensions/
│       ├── Filters/
│       ├── Middleware/
│       ├── Program.cs
│       ├── appsettings.json
│       └── Dockerfile
│
├── tests/
│   ├── API.Tests/
│   ├── Application.Tests/
│   └── Infrastructure.Tests/
│
└── docker-compose.yml
```

---

## API Endpoints

### Authentication

| Method | Endpoint               | Description                                   |
| ------ | ---------------------- | --------------------------------------------- |
| POST   | `/api/v1/auth/login`   | Authenticate user and generate JWT            |
| POST   | `/api/v1/auth/refresh` | Generate new access token using refresh token |

### Products

| Method | Endpoint                | Description            |
| ------ | ----------------------- | ---------------------- |
| GET    | `/api/v1/products`      | Get paginated products |
| GET    | `/api/v1/products/{id}` | Get product by ID      |
| POST   | `/api/v1/products`      | Create product         |
| PUT    | `/api/v1/products/{id}` | Update product         |
| DELETE | `/api/v1/products/{id}` | Delete product         |

### Items

| Method | Endpoint             | Description    |
| ------ | -------------------- | -------------- |
| GET    | `/api/v1/items`      | Get items      |
| GET    | `/api/v1/items/{id}` | Get item by ID |
| POST   | `/api/v1/items`      | Create item    |
| PUT    | `/api/v1/items/{id}` | Update item    |
| DELETE | `/api/v1/items/{id}` | Delete item    |

Protected endpoints require a valid JWT Bearer token.

---

## Authentication Flow

The API uses JWT-based authentication with refresh token support.

```text
Client
  |
  | POST /api/v1/auth/login
  | username + password
  v
API
  |
  | Validate credentials
  v
Generate Access Token + Refresh Token
  |
  v
Client
  |
  | Authorization: Bearer <access_token>
  v
Protected API endpoints
```

When the access token expires, the refresh token can be used to obtain a new access token.

### Demo Credentials

For assessment/demo purposes, the API contains a hardcoded demo credential:

```text
Username: admin
Password: Admin@123
```

> **Note:** These credentials are intentionally hardcoded for assessment/demo purposes. In a production application, users should be stored in a database and passwords must be securely hashed.

---

## Database

The application uses **SQL Server** with Entity Framework Core.

### Main Tables

```text
Product
  |
  | 1
  |
  | *
Item
```

### Product

```text
Id
ProductName
CreatedBy
CreatedOn
ModifiedBy
ModifiedOn
```

### Item

```text
Id
ProductId
Quantity
```

`Item.ProductId` is a foreign key referencing `Product.Id`.

Entity Framework Core migrations are included in the Infrastructure project.

The application automatically applies pending migrations during startup.

---

## Environment Setup

### Prerequisites

For local development:

* .NET 8 SDK
* SQL Server
* Visual Studio / VS Code
* Git

For Docker execution:

* Docker Desktop
* Docker Compose

---

## Running Locally

1. Clone the repository.

2. Navigate to the solution directory:

```powershell
cd C:\Assignment
```

3. Configure the SQL Server connection string in:

```text
src/ProductApiAssessment/appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ProductApiDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

4. Run the application:

```powershell
dotnet run --project src/ProductApiAssessment
```

5. Open Swagger:

```text
http://localhost:<application-port>/swagger
```

> The exact local development port may depend on the launch profile configured in `launchSettings.json`.

---

## Running with Docker

Docker Compose runs two containers:

```text
Product API
    |
    | productapi-network
    |
SQL Server
```

### Services

| Service     | Container              | Host Port |
| ----------- | ---------------------- | --------: |
| Product API | `productapi-app`       |    `5000` |
| SQL Server  | `productapi-sqlserver` |    `1433` |

Start the complete application:

```powershell
docker compose up --build
```

The API will be available at:

```text
http://localhost:5000
```

Swagger:

```text
http://localhost:5000/swagger
```

### Docker SQL Server Configuration

Inside Docker, the API connects to SQL Server using the Docker service name:

```text
Server=sqlserver,1433
Database=ProductApiDb
User Id=sa
Password=YourStrong@Passw0rd
TrustServerCertificate=True
```

> `sqlserver` is the Docker Compose service hostname. It should not be replaced with `localhost` from inside the API container.

### Docker SQL Server Credentials

```text
Username: sa
Password: YourStrong@Passw0rd
Database: ProductApiDb
Host: localhost
Port: 1433
```

From the host machine, SQL Server can therefore be accessed using:

```text
localhost,1433
```

> The SQL Server password above is a demo/development credential defined in `docker-compose.yml`. Production deployments should use secrets or environment-specific secure configuration.

### Verify Docker Containers

```powershell
docker ps
```

Expected services:

```text
productapi-app
productapi-sqlserver
```

---

## Swagger / OpenAPI

Swagger is configured using Swashbuckle and provides interactive API documentation.

Docker:

```text
http://localhost:5000/swagger
```

Swagger includes:

* API endpoint documentation
* Request and response schemas
* API version information
* JWT Bearer authentication

### Authorizing Swagger

1. Call:

```text
POST /api/v1/auth/login
```

2. Use:

```json
{
  "userName": "admin",
  "password": "Admin@123"
}
```

3. Copy the returned access token.

4. Click **Authorize** in Swagger.

5. Enter:

```text
Bearer <access_token>
```

6. Execute protected Product/Item endpoints.

---

## Testing

The solution contains three test projects:

```text
tests/
├── API.Tests/
├── Application.Tests/
└── Infrastructure.Tests/
```

### Test Coverage

* Application service unit tests
* Repository tests
* API integration tests
* Authentication flow testing
* Product API testing

Run all tests:

```powershell
dotnet test
```

Current test suite:

```text
Total:    14
Passed:   14
Failed:   0
Skipped:  0
```

---

## Deployment

The application is containerized using Docker.

### Build and Start

```powershell
docker compose up --build
```

### Run in Detached Mode

```powershell
docker compose up -d --build
```

### Stop Containers

```powershell
docker compose down
```

### Database Persistence

SQL Server uses a Docker named volume:

```text
sqlserver_data
```

This keeps database data persisted across container recreation.

---

## Logging

The application uses **Serilog** for structured logging.

Logs are written to:

```text
Console
Logs/log-<date>.txt
```

HTTP requests are logged through Serilog request logging middleware.

Database commands and application events can also be captured through the configured logging pipeline.

---

## Error Handling

A global exception handling middleware provides consistent API error responses.

The middleware handles application/domain exceptions such as:

* Validation errors
* Resource not found errors
* Unexpected application exceptions

This prevents exception-handling logic from being duplicated across controllers.

---

## Security

The solution implements several security practices:

* JWT Bearer authentication
* Short-lived access tokens
* Refresh token support
* Token validation for issuer and audience
* Input validation using FluentValidation
* CORS configuration
* HTTPS redirection
* Role-based authorization support
* SQL Server parameterized queries through Entity Framework Core
* Sensitive production credentials should be stored outside source control

> The demo credentials and Docker SQL password are included only to make the assessment easy to run. They should be replaced with secure secret management in production.

---

## Performance Considerations

The application follows several performance-oriented practices:

* Async/await throughout the application
* `AsNoTracking()` for read-only Entity Framework queries
* Pagination for collection endpoints
* Database indexing through entity configuration where appropriate
* Response compression
* Repository abstraction for controlled data access
* Efficient EF Core queries

---

## API Response and HTTP Status Codes

The API follows standard HTTP semantics.

Common responses include:

| Status Code               | Usage                              |
| ------------------------- | ---------------------------------- |
| 200 OK                    | Successful GET/PUT operation       |
| 201 Created               | Resource successfully created      |
| 204 No Content            | Successful delete/no response body |
| 400 Bad Request           | Validation or invalid request      |
| 401 Unauthorized          | Missing/invalid authentication     |
| 403 Forbidden             | Insufficient permissions           |
| 404 Not Found             | Resource does not exist            |
| 500 Internal Server Error | Unexpected server error            |

---

## API Versioning

The API uses URL-based versioning.

Current version:

```text
/api/v1/
```

Example:

```text
GET /api/v1/products
```

API versioning allows future versions to be introduced without breaking existing clients.

---

## Design Decisions

The solution intentionally separates responsibilities:

* **Controllers** handle HTTP concerns.
* **Services** contain application/business logic.
* **Repositories** handle data access.
* **Unit of Work** coordinates database operations.
* **Domain** contains core entities and domain concepts.
* **DTOs** prevent exposing domain entities directly through the API.
* **Middleware** handles cross-cutting concerns such as exception handling and logging.
* **Docker Compose** provides a reproducible API + SQL Server environment.

This structure makes the solution easier to test, maintain, and extend.
