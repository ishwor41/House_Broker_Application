# 🏠 HouseBroker Application

![.NET](https://img.shields.io/badge/.NET-7.0-blue)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-green)
![License](https://img.shields.io/badge/License-MIT-lightgrey)
![Tests](https://img.shields.io/badge/Tests-xUnit-orange)

---

## 📌 Project Overview

The HouseBroker project is built using **Clean Architecture principles**, ensuring separation of concerns, scalability, and maintainability.

The solution is divided into five main layers:

- **HouseBroker.API** → Presentation layer (Controllers, Swagger, API endpoints)
- **HouseBroker.Application** → Business logic layer (Services, Interfaces, DTOs)
- **HouseBroker.Domain** → Core domain layer (Entities, Enums, business rules)
- **HouseBroker.Infrastructure** → Infrastructure layer (Database access, Identity, external services)
- **HouseBroker.UnitTests** → Testing layer (Unit tests for services, controllers, and business logic using mocking frameworks)

---

## 🏗️ Architecture Flow

✔ API handles HTTP requests  
✔ Application contains business logic  
✔ Domain contains core rules  
✔ Infrastructure handles external dependencies  
✔ UnitTests validate business logic  

---

## ✨ Key Features

- JWT Authentication & Authorization
- Role-based access (HouseSeeker, Broker)
- Property Listing & Filtering
- Commission Calculation Logic
- Image Upload Support
- In-memory caching for performance
- Clean Architecture implementation
- Unit testing with xUnit & Moq

---

## 🚀 How to Run the Project

### 1. Clone the Repository
```bash
git clone <repo-url>
cd HouseBroker.API
dotnet restore HouseBrokerSolution.sln
For DB Migration
Option 1: EF Core Migrations
dotnet ef migrations add InitialCreate --project HouseBroker.Infrastructure --startup-project HouseBroker.API
dotnet ef database update --project HouseBroker.Infrastructure --startup-project HouseBroker.API
Option 2: SQL Script
Open SQL script from project folder
Run it in SQL Server Management Studio (SSMS)

🔐 Auth Module
➤ Register User
POST /api/Auth/register

Description:
Registers a new user in the system with a specific role.

Request Body:

{
  "email": "string",
  "password": "string",
  "role": "HouseSeeker | Broker"
}

Behavior:

Creates a new user
Assigns role (HouseSeeker or Broker)
Returns success message if registration is successful


➤ Login User
POST /api/Auth/login

Description:
Authenticates user and generates JWT token.

Request Body:

{
  "email": "string",
  "password": "string"
}

Response:

{
  "token": "jwt-token"
}

Behavior:

Validates user credentials
Generates JWT token
Token is used for authorization in protected endpoints


🏠 Property Module
➤ Get All Properties
Checks from auth token if the user is HouseSeeker or Broker
GET /api/Property

Description:
Returns list of properties.

Behavior:

If user is Broker → returns properties with commission details
If user is HouseSeeker → returns simplified property listing
Supports filtering (title, location, price, etc.)
Uses caching for performance
➤ Create Property
POST /api/Property

Authorization:
✔ Broker only

Description:
Creates a new property listing.

Request Body (FormData):

{
  "title": "string",
  "propertyType": "string",
  "location": "string",
  "price": 0,
  "features": "string",
  "propertyImage": "file"
}

Behavior:

Calculates commission automatically
Uploads property image
Saves property in database
Clears cached property data
➤ Get Property by ID
GET /api/Property/{id}

Description:
Returns single property by unique ID.

Behavior:

Returns property details
Includes image URL if available
Returns 404 if property not found


➤ Update Property
PUT /api/Property/{id}

Authorization:
✔ Broker only

Description:
Updates an existing property.

Request Body:

{
  "title": "string",
  "propertyType": "string",
  "location": "string",
  "price": 0,
  "features": "string"
}

Behavior:

Updates property details
Recalculates commission
Updates cache
(Optional) updates image if provided


➤ Delete Property
DELETE /api/Property/{id}

Authorization:
✔ Broker only

Description:
Deletes a property from the system.

Behavior:

Removes property from database
Clears cached data
Returns success response
