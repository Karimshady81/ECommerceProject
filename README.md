# 🛒 ECommerce API

A **personal backend project** built with **ASP.NET Core 8** following
**Clean / Layered Architecture principles**.
This project demonstrates how to structure a **scalable, maintainable,
and testable** e-commerce backend using real-world backend practices.

The goal of this project is learning, interview preparation, and
portfolio demonstration.

------------------------------------------------------------------------

## 🧱 Architecture Overview

This project follows a **Layered (Clean) Architecture** approach with
clear separation of concerns:

    API
    │
    Application
    │
    Domain
    │
    Infrastructure

### Layers Description

-   **API**
    -   ASP.NET Core Web API
    -   Controllers, Routing, Dependency Injection
    -   Authentication & Authorization (JWT)
-   **Application**
    -   Business logic and use cases
    -   Services and DTOs
    -   CQRS-style separation
    -   Validation rules
-   **Domain**
    -   Core business entities
    -   Domain models and interfaces
    -   Independent of frameworks and databases
-   **Infrastructure**
    -   Entity Framework Core
    -   Repositories
    -   Database access and migrations

------------------------------------------------------------------------

## ❓ Why Clean Architecture?

-   Separation of concerns
-   Highly testable codebase
-   Database and framework independence
-   Strong domain modeling
-   Scalable structure suitable for real-world applications

------------------------------------------------------------------------

## 🚀 Tech Stack

  -----------------------------------------------------------------------
  Layer                  Technologies
  ---------------------- ------------------------------------------------
  API                    ASP.NET Core 8, Web API, Controllers, Dependency
                         Injection

  Application            Services, DTOs, Business Rules

  Domain                 Entities, Interfaces

  Infrastructure         Entity Framework Core, MySQL, Repositories,
                         Migrations

  Security               JWT Authentication & Authorization

  Testing                Unit Tests & Integration Tests
  -----------------------------------------------------------------------

------------------------------------------------------------------------

## ✨ Features

### Currently Implemented

-   Clean / Layered Architecture structure
-   Authentication & Authorization using **JWT**
-   Users & Roles management
-   Products module (CRUD)
-   Categories
-   Cart & Orders system
-   Checkout flow
-   Global exception handling
-   Input validation using **FluentValidation**
-   Unit testing
-   Integration testing

------------------------------------------------------------------------

## 🧪 Testing

-   **Unit Tests**
    -   Business logic and services
-   **Integration Tests**
    -   API endpoints
    -   Controllers and infrastructure interaction

------------------------------------------------------------------------

## 🗄 Database

-   **MySQL**
-   **Entity Framework Core**
-   Code-First approach
-   Migrations for schema management

------------------------------------------------------------------------

## 🔐 Security & Validation

-   JWT-based authentication
-   Role-based authorization
-   Global exception handling middleware
-   Request validation using FluentValidation

------------------------------------------------------------------------

## ▶️ Running the Project

### Prerequisites

-   .NET 8 SDK
-   MySQL Server

### Steps

1.  Clone the repository

``` bash
git clone https://github.com/your-username/ecommerce-api.git
```

2.  Update the connection string in `appsettings.json`

3.  Apply migrations

``` bash
dotnet ef database update
```

4.  Run the application

``` bash
dotnet run
```

5.  Access Swagger UI

```{=html}
<!-- -->
```
    https://localhost:{port}/swagger

------------------------------------------------------------------------

## 📚 What I Learned

-   Applying Clean Architecture in a real backend project
-   Structuring scalable ASP.NET Core applications
-   Implementing JWT authentication and authorization
-   Writing testable services
-   Using Entity Framework Core with MySQL
-   Handling validation and global errors properly
