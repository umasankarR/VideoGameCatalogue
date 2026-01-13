# Video Game Catalogue

A .NET 10 application built with Clean Architecture principles for managing a video game catalogue.

## Architecture

This solution follows **Clean Architecture** principles with clear separation of concerns:

```
VideoGameCatalogue/
├── src/
│   ├── Domain/          # Core business entities and logic
│   ├── Application/     # Use cases, interfaces, and business rules
│   ├── Infrastructure/  # Data access, external services
│   └── WebAPI/          # API endpoints and presentation layer
├── Directory.Build.props                    # Common build properties
├── Directory.Packages.props                 # Centralized package version management
└── VideoGameCatalogue.sln                   # Solution file
```

## Technology Stack

- **.NET 10** - Latest .NET framework
- **Entity Framework Core 10** - ORM for database access
- **MediatR** - CQRS pattern implementation
- **FluentValidation** - Input validation
- **AutoMapper** - Object-to-object mapping
- **SQL Server** - Database provider

## Project Dependencies

The dependency flow follows Clean Architecture rules:

```
Domain (no dependencies)
   ↑
Application (depends on Domain)
   ↑
Infrastructure (depends on Application)
   ↑
WebAPI (depends on Application & Infrastructure)
```

## Package Management

This solution uses **Central Package Management** (CPM) via `Directory.Packages.props`:
- All package versions are defined centrally
- Projects reference packages without specifying versions
- Ensures consistency across all projects

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server (LocalDB, Express, or Full)

### Build the Solution
```bash
dotnet build
```

### Run the API
```bash
dotnet run --project src/WebAPI
```

## Clean Architecture Layers

### 1. Domain Layer
- Contains enterprise business rules
- Entities, Value Objects, Domain Events
- No dependencies on other layers

### 2. Application Layer
- Contains application business rules
- Use cases (Commands/Queries via MediatR)
- Interfaces for infrastructure
- DTOs and mapping profiles

### 3. Infrastructure Layer
- Implements interfaces from Application layer
- Database context and configurations
- External service integrations
- Data persistence

### 4. WebAPI Layer
- REST API endpoints
- Dependency injection configuration
- Middleware and filters
- API documentation (Swagger)

## Development Guidelines

1. **Domain Layer**: Keep it pure - no external dependencies
2. **Application Layer**: Define interfaces, implement business logic
3. **Infrastructure Layer**: Implement data access and external services
4. **WebAPI Layer**: Handle HTTP concerns only

## Next Steps

To complete the implementation:
1. Define domain entities (Game, Genre, Platform, etc.)
2. Create application use cases (Commands/Queries)
3. Implement repository pattern in Infrastructure
4. Add API controllers in WebAPI
5. Configure database connection and migrations
6. Add authentication and authorization
7. Implement logging and error handling
8. Write unit and integration tests

