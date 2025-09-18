# StudentManagement (ASP.NET Core, .NET 9, MongoDB, Aspire)

A minimal Student Management REST API backed by MongoDB, orchestrated for local development with .NET Aspire AppHost.

## Overview
- .NET 9, C# 13
- ASP.NET Core Web API with Swagger/OpenAPI
- MongoDB persistence (container provisioned by Aspire)
- Opinionated result/response pattern using `Result` → `ApiResponse`
- Optional console app for Azure Service Bus receiving

## Project structure
## Prerequisites
- .NET SDK 9.0+
- Docker Desktop (required by Aspire to run MongoDB as a container)

## Quick start (recommended)
1. Restore and build:
2. Run the Aspire AppHost:
3. After services start, open the API Swagger UI:
- Find the assigned port in the AppHost console output.
- Navigate to: `http://localhost:<port>/swagger`

Aspire will:
- Start a MongoDB 7 container with credentials
- Create the `myFirstDatabase` database
- Start the API and inject the database connection via environment variables

## Running the API without AppHost
Provide a MongoDB instance and configure the API via appsettings or environment variables.

Example `appsettings.Development.json`:

Then:

Environment variable equivalents:
- `StudentStoreDatabaseSettings__ConnectionString`
- `StudentStoreDatabaseSettings__DatabaseName`
- `StudentStoreDatabaseSettings__StudentCoursesCollectionName`

Note: When using AppHost, the connection is provided for you; you typically only see:
- `StudentStoreDatabaseSettings__DatabaseName`
- `StudentStoreDatabaseSettings__StudentCoursesCollectionName`

## API
Base route: `/api/students`

- GET `/api/students` → List students
- GET `/api/students/{id}` → Get by id
- POST `/api/students` → Create
- PUT `/api/students/{id}` → Replace
- DELETE `/api/students/{id}` → Delete

Swagger: `/swagger`

### Request/Response shape
Responses are wrapped in:

Example Student document (fields may vary by your model):

### cURL examples
- Create:
````````

# Response
````````markdown
````````

- List:
````````


# Response
````````markdown
````````

- Get by id:
````````


# Response
````````markdown
````````

- Update:
````````


# Response
````````markdown
````````

- Delete:
````````


# Response
````````markdown
````````

## Telemetry
`StudentManagement.ServiceDefaults` includes OpenTelemetry dependencies. If you connect an OTLP endpoint, the API can export traces/metrics. This is optional for local dev.

## Optional: ServiceBusReceiver
A separate console app for consuming messages from Azure Service Bus.

- Set environment variables:
- `AZURE_SERVICEBUS_CONNECTION_STRING`
- `AZURE_SERVICEBUS_QUEUE_OR_TOPIC` (and subscription if needed)
- Run:

## Troubleshooting
- Docker not running: Aspire will fail to start MongoDB. Ensure Docker Desktop is running.
- Port already in use: Stop conflicting processes or let Aspire allocate a different port.
- Cannot connect to MongoDB when running API standalone: verify `ConnectionString` and that Mongo is reachable.

## License
MIT (or update to your chosen license)