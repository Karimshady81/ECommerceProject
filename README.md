# 📦 ECommerceProject  
A modular **Clean Architecture** E-Commerce backend built with **ASP.NET Core**, following domain-driven principles and separation of concerns.  
This project demonstrates a real-world, production-ready project structure suitable for learning, interviews, and portfolio use.

## 🏗️ Architecture Overview

This solution follows a layered Clean Architecture pattern:

```
ECommerceProject
│
├── ECommerceAPI               → Presentation Layer (Controllers, Endpoints)
│
├── ECommerceAPI.Application   → Application Layer (CQRS, DTOs, Services)
│
├── ECommerceAPI.Domain        → Core Domain (Entities, Enums, Interfaces)
│
└── ECommerceAPI.Infrastructure → Infrastructure Layer (EF Core, Repositories, DB)
```

### ✔️ Why Clean Architecture?
- Separation of concerns  
- Testability  
- Database independence  
- Strong domain modeling  
- Scalable codebase for real applications  

## 🚀 Tech Stack

| Layer | Technologies |
|-------|-------------|
| **API** | ASP.NET Core Web API, Controllers, Routing, Dependency Injection |
| **Application** | Services, DTOs, CQRS-style logic, Business rules |
| **Domain** | Domain Entities, Aggregates, Interfaces |
| **Infrastructure** | Entity Framework Core, Repositories, Migrations |

## 🧩 Features (Current & Planned)

### Currently implemented
- Project structure according to Clean Architecture  
- Layered separation (API, Application, Domain, Infrastructure)  
  

### Coming soon
- 🔐 Authentication & Authorization (JWT + Identity)  
- 🛒 Products Module (CRUD, filtering, sorting)  
- 📦 Orders & Cart System  
- 👤 Users & Roles  
- 💳 Checkout / Payment Integration  
- 📁 File Uploads (images, product gallery)  
- 🌐 Global exception handling  
- 🎯 Validation using FluentValidation  
- 📊 Logging & auditing support  

## 📂 Folder Structure

```
📦 ECommerceProject
 ┣ 📂 ECommerceAPI
 ┣ 📂 ECommerceAPI.Application
 ┣ 📂 ECommerceAPI.Domain
 ┣ 📂 ECommerceAPI.Infrastructure
 ┣ 📄 .gitignore
 ┗ 📄 ECommerceProject.sln
```

## ▶️ How to Run the Project

1. **Clone the repo**  
   ```
   git clone https://github.com/Karimshady81/ECommerceProject.git
   ```

2. **Open the solution**  
   Open the `.sln` file in **Visual Studio 2022**.

3. **Restore dependencies**  
   ```
   dotnet restore
   ```

4. **Apply migrations (if implemented)**  
   ```
   dotnet ef database update
   ```

5. **Run the API**
   ```
   dotnet run --project ECommerceAPI
   ```

## 📌 Roadmap
- Add authentication (JWT)  
- Implement Product module  
- Add Category module  
- Add Cart & Order flow  
- Add Payments integrations  
- Add Swagger documentation  
- Add caching (Redis)  
- Add unit/integration tests  

## 🤝 Contributing
This is a learning project — contributions, suggestions, and improvements are welcome!

