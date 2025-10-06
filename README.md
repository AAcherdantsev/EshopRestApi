# EshopRestApi

**EshopRestApi** is a demo e-commerce project built with **.NET Aspire**.  
It provides an API for managing products (adding, retrieving lists, getting by id, updating stock quantities) and orchestrates supporting services with Docker: **Kafka**, **MS SQL**, and **Kafbat UI**.

API versioning has been implemented. In the second version, you can get a list of products with pagination, and when updating the product quantity, **Kafka** is used for asynchronous execution.


---

# Features
* Manage products: add, list (with pagination), get by id, update stock quantities (synchronous and asynchronous)
* API documentation available via Swagger UI
* API versioning (v1 and v2)
* MS SQL database for product storage
* Message processing powered by Apache Kafka
* Real-time monitoring with Kafbat UI
* Automatic database migration & product seeding

# Technologies
* .NET 8 + ASP.NET Core
* .NET Aspire — service orchestration
* FastEndpoints — API design aligned with SOLID principles
* AutoMapper — DTO ↔ Domain mapping
* MS SQL — relational database
* Apache Kafka + Kafbat UI — streaming & monitoring
* Docker — containerization

## Quick Start

1. **Installing dependencies**  

    This solution uses the .NET 8 SDK, .NET Aspire workload, and Docker. You can install it yourself or simply use the `setup` script from the **scripts** folder. To do this, open a terminal in the root folder of this project and run the command and follow the instructions:
   ```bash
   sudo bash scripts/setup.sh       # Linux/macOS
   ```
   
   ```powershell
   .\setup.ps1                      # Windows
   ```

2. **Run unit tests (optional)**

   To run unit tests, you can use your IDE tools or, while in the root folder of this project, execute this command in the terminal:
   ```bash 
   dotnet test
   ```

3. **Configure SQL superuser password**

   In order for .NET Aspire to run SQL Server, you must provide the superuser password that will be used when connecting to the database. The password must be at least 8 characters long and contain characters from three of the following four sets: Uppercase letters, Lowercase letters, Base 10 digits, and Symbols. To do this, execute the commands (and replace `MyStrongP@ssw0rd` with your password): 
   ```bash
   cd src/Shop.Products.AppHost
   dotnet user-secrets set "Parameters:sql-password" "MyStrongP@ssw0rd"
   ```

4. **Run .NET Aspire**

   When everything is ready, run the `Shop.Products.AppHost` project through your IDE or using this command in the terminal:

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
