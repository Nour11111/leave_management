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


