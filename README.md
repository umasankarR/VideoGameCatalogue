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
- SQL Server (LocalDB, Express)

### Build the Solution
```bash
dotnet build
```

### Run the API
```bash
dotnet run --project src/WebAPI
```
