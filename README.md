# EshopRestApi

**EshopRestApi** is a demo e-commerce project built with **.NET Aspire**.  
It provides an API for managing products (adding, retrieving lists, getting by id, updating stock quantities) and orchestrates supporting services with Docker: **Kafka**, **MS SQL**, and **Kafbat UI**.  

---

# Features
* Manage products: add, list (with pagitation), get by id, update stock quantities
* API documentation available via Swagger UI
* Message processing powered by Apache Kafka
* Real-time monitoring with Kafbat UI
* Automatic database migration & product seeding

# Technologies
* .NET 8 + ASP.NET Core
* .NET Aspire — service orchestration
* FastEndpoints — API design aligned with SOLID principles
* AutoMapper — DTO ↔ Domain mapping
* MSSQL — relational database
* Apache Kafka + Kafbat UI — streaming & monitoring
* Docker — containerization

## Quick Start

1. **Run setup script**  
   ```bash
   sudo bash scripts/setup.sh       # Linux/macOS
   ```

   ```powershell
   .\setup.ps1                      # Windows
   ```

    The setup script installs:
    * .NET 8 SDK
    * .NET Aspire workload
    * Docker 

2. **Configure SQL superuser password**
   ```bash
   cd src/Shop.Products.AppHost
   dotnet user-secrets set "Parameters:sql-password" "MyStrongP@ssw0rd"
   ```

3. **Run .NET Aspire**
   ```bash
   dotnet run --project Shop.Products.AppHost.csproj
   ```

   After startup:
    * .NET Aspire will start Kafka and Kafbat UI
    * MS SQL will be automatically provisioned
    * All EF Core migrations will be applied automatically
    * A set of sample products will be seeded into the database

   <img width="1570" height="968" alt="image" src="https://github.com/user-attachments/assets/84a62bc2-9033-4d06-8fd3-93ffe003869a" />
   <img width="1570" height="968" alt="image" src="https://github.com/user-attachments/assets/db570847-bfde-4a49-b34a-60acd47e873a" />    
