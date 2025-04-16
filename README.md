# Leave Management System

## Description

A system for managing employee leave requests.

## Features

* Employee Management:
    * Create, read, update, and delete employees.
* Leave Request Management:
    * Create, read, update, and delete leave requests.
    * Validation of leave requests (overlapping dates, maximum annual leave, etc.).
* API Documentation (Swagger).
* Docker support for easy setup.

## Technologies Used

* .NET 8
* ASP.NET Core
* Entity Framework Core 
* FluentValidation
* AutoMapper
* Dynamic LINQ
* Docker

## Getting Started

### Prerequisites

* .NET 8 SDK: [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)
* Docker Desktop: [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop) 

### Setup Instructions

**Using Docker (Recommended):**

1.  Clone the repository:

    ```bash
    git clone <repository_url>
    cd leave-management-system  # Change to the project directory
    ```
2.  Build and run the API using Docker Compose:

    docker-compose up -d
    ```

**Without Docker:**

1.  Clone the repository:

    git clone <repository_url>
    cd leave-management-system
    ```
2.  Navigate to the  project directory
3.  Restore the NuGet packages:

    ```bash
    dotnet restore
    ```
4.  Build the project:

    ```
    dotnet build -c Release
    ```
5.  Update the database. You might need to create an initial migration:

    ```
    dotnet ef database update
    ```
6.  Run the API:

    ```
    dotnet run --project LeaveManagement.csproj
    ```
7.  The API will be running at the URL shown in the console (e.g., `http://localhost:5000`).

### Running the API

* The API documentation (Swagger) will be available at `http://localhost:8000/swagger``.

### Using the API

* You can use Postman or any other HTTP client to interact with the API.
* See the Swagger documentation for available endpoints and request/response formats.


## LeaveRequest Status

* Pending
* Approved
* Rejected

## LeaveRequest Type

* Annual
* Sick
* Other


##   Swagger Screenshot
![image](https://github.com/user-attachments/assets/a91b7773-2454-4424-8828-01001dc6c343)


##   Self-Review

###   Notes on Improvements

In this project, we focused on building a robust and maintainable Leave Management API, adhering to Clean Architecture principles. Key aspects of our work include:

* **Clean Architecture:** The project is structured to separate concerns, with a clear separation between the domain layer, application layer, and infrastructure layer. This promotes testability, maintainability, and scalability.
* **Business Logic and Rules:** We implemented core business logic and rules, such as:
    * Employee management (create, read, update, delete).
    * Leave request management (create, read, update, delete).
    * Validation of leave requests, including date ranges and status transitions.
* **Data Transfer Objects (DTOs):** We used DTOs to define the data contracts for API requests and responses, ensuring a clean separation between the API and the domain models.
* **AutoMapper:** AutoMapper was used to map between DTOs and domain entities, reducing boilerplate code and improving maintainability.
* **Exception Handling:** We implemented exception handling to manage errors gracefully, but this could be further improved by implementing a global exception handler and returning structured error responses.
* **Logging:** We incorporated logging using ILogger, but more detailed logging, including request/response details and structured logging, could be beneficial.
* **Validation:** We used FluentValidation to enforce validation rules for incoming requests, ensuring data integrity.  Validation rules could be expanded to include more complex business rules, such as checking for overlapping leave requests and enforcing maximum annual leave days.
* **Asynchronous Operations:** All database interactions and I/O operations are performed asynchronously to prevent blocking.
* **Documentation:** Swagger is used to document the API endpoints, but the documentation could be reviewed for completeness and clarity.
Potential areas for improvement include:
* Implementing authentication and authorization to secure the API.
* Adding more comprehensive unit and integration tests.

###   Test Plans

We plan to implement comprehensive unit and integration tests to ensure the API's quality and reliability.  This will involve:

* **Unit Tests:**
    * Validating all validation rules in command and query validators.
    * Testing command and query handlers in isolation using mocking to verify correct behavior.
    * Testing repository methods to ensure correct database interactions, using an in-memory database.
* **Integration Tests:**
    * Testing all API endpoints (controllers) to verify correct HTTP status codes, headers, and response bodies for various scenarios, including valid and invalid data.
    * Testing the API's interaction with the database using a test database to ensure data persistence and transaction handling.

The testing strategy will cover:

* All validation logic.
* The behavior of all API endpoints.
* The interaction between different layers of the application (controllers, services, repositories).
* Error handling and edge cases.

We will use xUnit as our testing framework . 

