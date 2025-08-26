# Todo API - .NET Core Web API

A comprehensive Todo application REST API built with .NET 8, featuring JWT authentication, Entity Framework Core, SQL Server, and following best practices including Repository pattern, AutoMapper, and global exception handling.

## 🚀 Features

### Authentication APIs

- **POST /api/auth/signup** - User registration with validation
- **POST /api/auth/login** - User login with JWT token response

### Protected Todo APIs (Require JWT Authentication)

- **GET /api/todos** - Get user's todos with pagination
- **GET /api/todos/{id}** - Get specific todo by ID
- **POST /api/todos** - Create new todo
- **PUT /api/todos/{id}** - Update existing todo
- **DELETE /api/todos/{id}** - Delete todo
- **GET /api/todos/stats** - Get user's todo statistics

## 🏗️ Technical Architecture

### Core Technologies

- **.NET 8** - Latest LTS version
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core** - ORM with Code-First approach
- **SQL Server** - Primary database
- **JWT Authentication** - Secure token-based authentication
- **AutoMapper** - Object-to-object mapping
- **BCrypt.NET** - Password hashing
- **Swagger/OpenAPI** - API documentation

### Design Patterns & Practices

- **Repository Pattern** - Generic repository implementation
- **Unit of Work Pattern** - Transaction management
- **Dependency Injection** - Loose coupling
- **Global Exception Handling** - Centralized error management
- **Input Validation** - Data annotation validation
- **Pagination** - Efficient data retrieval
- **Auto-Migration** - Database schema management

## 📋 Prerequisites

Before running this application, ensure you have the following installed:

1. **.NET 8 SDK or later**

   - Download from: https://dotnet.microsoft.com/download
   - Verify installation: `dotnet --version`

2. **SQL Server or SQL Server Express LocalDB**

   - SQL Server Express: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - LocalDB is included with Visual Studio

3. **Visual Studio 2022** or **Visual Studio Code** (recommended)

   - Visual Studio: https://visualstudio.microsoft.com/
   - VS Code: https://code.visualstudio.com/

4. **Git** (for cloning the repository)
   - Download from: https://git-scm.com/

## ⚙️ Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd todo-api
```

### 2. Navigate to Project Directory

```bash
cd TodoApi
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Update Database Connection String (Optional)

Edit `appsettings.json` and update the connection string if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TodoApiDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 5. Apply Database Migrations

The application will automatically apply migrations on startup, but you can also run them manually:
```bash
dotnet ef database update
```

### 6. Build the Project

```bash
dotnet build
```

### 7. Run the Application

```bash
dotnet run
```

The API will be available at:

- **HTTPS**: https://localhost:7001
- **HTTP**: http://localhost:5000
- **Swagger UI**: https://localhost:7001 (or http://localhost:5000)

## 📱 API Usage Examples

### Authentication

#### Register a New User

```http
POST /api/auth/signup
Content-Type: application/json

{
  "username": "nakib",
  "email": "nakib@example.com",
  "password": "SecurePassword123"
}
```

#### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "nakib@example.com",
  "password": "SecurePassword123"
}
```

**Response:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "nakib",
  "email": "nakib@example.com",
  "expires": "2024-01-08T10:30:00Z"
}
```

### Todo Operations (Require Authentication)

#### Get Todos with Pagination

```http
GET /api/todos?page=1&pageSize=10
Authorization: Bearer <your-jwt-token>
```

#### Create a New Todo

```http
POST /api/todos
Authorization: Bearer <your-jwt-token>
Content-Type: application/json

{
  "title": "Complete project documentation",
  "description": "Write comprehensive README and API documentation"
}
```

#### Update a Todo

```http
PUT /api/todos/1
Authorization: Bearer <your-jwt-token>
Content-Type: application/json

{
  "title": "Complete project documentation",
  "description": "Write comprehensive README and API documentation",
  "isCompleted": true
}
```

#### Delete a Todo

```http
DELETE /api/todos/1
Authorization: Bearer <your-jwt-token>
```

#### Get Todo Statistics

```http
GET /api/todos/stats
Authorization: Bearer <your-jwt-token>
```

**Response:**

```json
{
  "totalTodos": 15,
  "completedTodos": 8,
  "pendingTodos": 7,
  "completionRate": 53.33
}
```

## 🧪 Testing with Postman

### Import the Postman Collection

1. Open Postman
2. Click "Import" button
3. Select the `Todo_API.postman_collection.json` file from the repository
4. The collection will include pre-configured requests for all endpoints

### Environment Setup

Create a new environment in Postman with the following variables:

- `baseUrl`: `https://localhost:56771` (or your API URL)
- `authToken`: (will be set automatically after login)

### Authentication Flow

1. Use the "Register" request to create a new account
2. Use the "Login" request to authenticate and get a JWT token
3. The token will be automatically set in the environment variable
4. All subsequent requests will use this token automatically

## 📁 Project Structure

```
TodoApi/
├── Controllers/           # API Controllers
│   ├── AuthController.cs
│   └── TodosController.cs
├── Data/                  # Database Context
│   └── Migrations/
│   └── TodoDbContext.cs
├── DTOs/                  # Data Transfer Objects
│   ├── AuthDtos.cs
│   └── TodoDtos.cs
├── Mappings/              # AutoMapper Profiles
│   └── MappingProfile.cs
├── Middleware/            # Custom Middleware
│   ├── GlobalExceptionMiddleware.cs
│   └── MiddlewareExtensions.cs
├── Models/                # Entity Models
│   ├── TodoItem.cs
│   └── User.cs
├── Repositories/          # Repository Pattern
│   ├── GenericRepository.cs
│   ├── IGenericRepository.cs
│   ├── ITodoRepository.cs
│   ├── IUnitOfWork.cs
│   ├── IUserRepository.cs
│   ├── TodoRepository.cs
│   ├── UnitOfWork.cs
│   └── UserRepository.cs
├── Services/              # Business Logic
│   ├── AuthService.cs
│   ├── IAuthService.cs
│   ├── ITodoService.cs
│   └── TodoService.cs
├── appsettings.json       # Configuration
├── Program.cs             # Application Entry Point
└── TodoApi.csproj         # Project File
```

## 🔧 Configuration

### JWT Settings

Update JWT configuration in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "your-secret-key-at-least-32-characters",
    "Issuer": "TodoApi",
    "Audience": "TodoApiUsers"
  }
}
```

### Database Configuration

Update connection string for your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=TodoApiDb;Trusted_Connection=true;"
  }
}
```

## 🐛 Troubleshooting

### Common Issues

1. **"dotnet command not found"**

   - Ensure .NET 8 SDK is installed
   - Add .NET to your system PATH

2. **Database Connection Issues**

   - Verify SQL Server is running
   - Check connection string in appsettings.json
   - Ensure database permissions are correct

3. **Migration Issues**

   - Delete existing migrations: `rm -rf Migrations/`
   - Create new migration: `dotnet ef migrations add InitialCreate`
   - Update database: `dotnet ef database update`

4. **JWT Token Issues**
   - Ensure JWT Key is at least 32 characters
   - Check token expiration
   - Verify token format in Authorization header

### Logs and Debugging

Enable detailed logging in `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

## 🔒 Security Features

- **Password Hashing**: BCrypt with salt
- **JWT Authentication**: Secure token-based auth
- **User Isolation**: Users can only access their own todos
- **Input Validation**: Comprehensive data validation
- **HTTPS**: Enforced in production
- **CORS**: Configurable cross-origin requests

## 📊 Performance Features

- **Pagination**: Efficient data loading
- **Entity Framework Tracking**: Optimized queries
- **Connection Pooling**: Database connection management
- **Async/Await**: Non-blocking operations

## 🧪 Testing

### Unit Testing (Future Enhancement)

```bash
# Create test project
dotnet new xunit -n TodoApi.Tests

# Add test references
dotnet add TodoApi.Tests/TodoApi.Tests.csproj reference TodoApi/TodoApi.csproj
```

### Integration Testing (Future Enhancement)

```bash
# Run integration tests
dotnet test
```

## 🚀 Deployment

### IIS Deployment

1. Publish the application: `dotnet publish -c Release`
2. Copy published files to IIS wwwroot
3. Configure connection string for production database
4. Update JWT settings with production keys

### Docker Deployment (Future Enhancement)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TodoApi.csproj", "."]
RUN dotnet restore "./TodoApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TodoApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
```

## 📈 Future Enhancements

- [ ] Unit and Integration Tests
- [ ] Docker Support
- [ ] Redis Caching
- [ ] Rate Limiting
- [ ] API Versioning
- [ ] Health Checks
- [ ] Logging with Serilog
- [ ] Email Verification
- [ ] Password Reset
- [ ] Role-based Authorization
- [ ] Todo Categories/Tags
- [ ] Todo Sharing
- [ ] File Attachments
- [ ] Todo Due Dates
- [ ] Push Notifications

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📞 Support

For questions and support:

- Create an issue in the GitHub repository

---

**Built with ❤️ using .NET 8 and Entity Framework Core**
