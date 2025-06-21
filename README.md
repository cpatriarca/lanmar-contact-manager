# 📇 Contact Manager App

A simple .NET Core MVC web application that provides basic CRUD functionality for managing contacts. Developed as part of a technical assessment.

## ✅ Features

- Create, read, update, and delete (CRUD) contacts
- Autocomplete style search contacts by name, company, or email
- Bootstrap-based UI with clean layout
- EF Core with SQLite database
- Unit tests for service and controller layers

## 🛠 Technologies

- .NET 8
- ASP.NET Core MVC
- Entity Framework Core
- SQLite
- xUnit, Moq for testing
- Bootstrap 5

---

## 🚀 Getting Started

### 1. Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or compatible version
- Git
- IDE such as Visual Studio 2022+ or Visual Studio Code

### 2. Clone the Repository

```bash
git clone https://github.com/cpatriarca/lanmar-contact-manager.git
cd lanmar-contact-manager
```

### 3. Build the Solution

```bash
dotnet build
```

### 4. Run the Application

```bash
dotnet run --project ContactManager.Api
```

Open a browser and go to:

```
http://localhost:5000
```

(or `https://localhost:5001` for HTTPS if enabled)

### 5. Run the Tests

```bash
dotnet test
```

---

## 💾 Database and Seed Data

- The app uses SQLite for simplicity and portability.
- On first run, the app will automatically create the database (`contactmanager.db`) in the project root.
- If the database is empty, the app will seed a few default contacts.
- To reset data, simply delete `contactmanager.db` and re-run the app.

---

## 🧪 Testing

- All business logic is covered by unit tests:
  - `ContactService` tests
  - `ContactController` tests
- Tests are located in the `ContactManager.Tests` project.
- Run all tests with:

```bash
dotnet test
```

---

## 🗂 Project Structure

```
ContactManager.sln
│
├── ContactManager.Api           # MVC Web Application
│   ├── Controllers
│   ├── Views
│   └── wwwroot
│
├── ContactManager.Application   # DTOs and Services
│   ├── Models
│   └── Services
│
├── ContactManager.Infrastructure # EF Core DbContext and Entities
│
└── ContactManager.Tests         # Unit Tests
```

---

## 🧭 Navigation

- `/Contact/Index` - List all contacts
- `/Contact/Create` - Add new contact
- `/Contact/Edit/{id}` - Edit a contact
- `/Contact/Delete/{id}` - Delete a contact
- `/Contact/Search` - Ajax search (returns partial view)

---

## 📎 Notes

- Uses `EnsureCreated()` to initialize the SQLite database.
- Ideal for local testing and demo purposes — not production-grade.
- Logging is enabled via `ILogger<ContactController>`.

---

## 📃 License

This project is open-source and available under the MIT License.

---

## 🤝 Contact

Made with ❤️ by Christian Patriarca.
