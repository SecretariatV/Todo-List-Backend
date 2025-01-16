# Todo-List Server

This project is a C# backend server designed for managing a TODO application. It uses Entity Framework Core for database management, ASP.NET Core for the web API, and includes features like authentication, user management, and todo operations.

## Features

- **User Management**: Create, read, update, and delete user accounts.
- **Authentication**: Secure user authentication using passwords.
- **TODO Management**: Add, update, retrieve, and delete todos for each user.
- **Database Integration**: Built with Entity Framework Core, compatible with SQL databases.

## Technologies Used

- **ASP.NET Core**: Framework for building the web API.
- **Entity Framework Core**: ORM for database management.
- **SQL Server**: Database for storing user and todo data.
- **Dependency Injection**: Built-in ASP.NET Core DI container.
- **AutoMapper**: For object mapping between DTOs and entities.

## Getting Started

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/SecretariatV/Hiring-Backend.git
   cd Hiring-Backend
   ```

2. Update the `appsettings.json` file with your database connection string:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=<your-server>;Database=<your-database>;User Id=<your-username>;Password=<your-password>;"
   }
   ```

3. Apply database migrations:

   ```bash
   dotnet ef database update
   ```

4. Run the application:

   ```bash
   dotnet run
   ```

5. Access the server locally at `https://localhost:5000`.

## Endpoints

## Endpoints

### Authentication

| Method | Endpoint             | Description          |
| ------ | -------------------- | -------------------- |
| POST   | `/api/auth/login`    | Log in a user.       |
| POST   | `/api/auth/register` | Register a new user. |

### Todos

| Method | Endpoint          | Description        |
| ------ | ----------------- | ------------------ |
| GET    | `/api/todos`      | Get all todos.     |
| POST   | `/api/todos`      | Create a new todo. |
| PUT    | `/api/todos/{id}` | Update a todo.     |
| DELETE | `/api/todos/{id}` | Delete a todo.     |

## Project Structure

```
├── Controllers
│   ├── AuthController.cs
│   └── TodoController.cs
├── Data
│   ├── AppDbContext.cs
│   ├── Entities
│   │   ├── CoreEntity.cs
│   │   ├── UserEntity.cs
│   │   └── TodoEntity.cs
├── DTOs
│   ├── RequestDto.cs
├── Services
│   ├── UserService.cs
│   └── TodoService.cs
├── Middleware
│   └── AuthMiddleware.cs
├── Program.cs
```
