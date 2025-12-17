# PhotoBooker

A full-stack photography booking platform that connects clients with professional photographers. Built with ASP.NET Core Web API and React with TypeScript.

## 🚀 Features

- **User Authentication & Authorization**: JWT-based authentication with role management (Client/Photographer)
- **Photographer Profiles**: Comprehensive profiles with portfolios and packages
- **Portfolio Management**: Upload and showcase photography work organized by categories

### 🔮 Future Implementations

- **Booking System**: Request and manage photography sessions
- **Availability Management**: Photographers can set their available time slots
- **Package Management**: Create and offer different photography service packages

## 🏗️ Architecture

This project follows Clean Architecture principles with clear separation of concerns:

```
PhotoBooker/
├── PhotoBooker.API/          # REST API layer (Controllers, Swagger configuration)
├── PhotoBooker.Application/  # Business logic (Services, DTOs, Interfaces)
├── PhotoBooker.Domain/       # Core domain entities and business rules
├── PhotoBooker.Infrastructure/ # Data access (EF Core, Repositories)
└── PhotoBooker.Client/       # React + TypeScript frontend
```

## 🛠️ Tech Stack

### Backend
- **Framework**: ASP.NET Core 9.0
- **Database**: SQLite with Entity Framework Core
- **Authentication**: JWT Bearer tokens
- **API Documentation**: Swagger/OpenAPI
- **Architecture**: Clean Architecture with Repository Pattern

### Frontend
- **Framework**: React 19
- **Language**: TypeScript
- **Build Tool**: Vite
- **Routing**: React Router v7

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (v18 or higher)
- npm or yarn

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/karinaconstandache/PhotoBooker.git
cd PhotoBooker
```

### 2. Backend Setup

```bash
# Restore dependencies
dotnet restore

# Apply database migrations
cd PhotoBooker.Infrastructure
dotnet ef database update

# Build the solution
cd ..
dotnet build
```

### 3. Frontend Setup

```bash
cd PhotoBooker.Client
npm install
```

### 4. Run the Application

#### Option 1: Run individually

**Backend:**
```bash
# From the root directory
dotnet run --project PhotoBooker.API/PhotoBooker.API.csproj
```
The API will be available at `https://localhost:5001` and `http://localhost:5000`

**Frontend:**
```bash
cd PhotoBooker.Client
npm run dev
```
The client will be available at `http://localhost:5173`

#### Option 2: Run with VS Code tasks

If you're using VS Code, you can use the configured tasks:

- Press `Ctrl+Shift+P` (or `Cmd+Shift+P` on Mac)
- Type "Run Task" and select it
- Choose `start-all` to run both API and Client simultaneously

Or run individually:
- `build-api` - Build the API project
- `run-api` - Run the API server
- `run-client` - Run the frontend development server

## 📚 API Documentation

Once the backend is running, access the Swagger UI at:
- `https://localhost:5001/swagger` or `http://localhost:5000/swagger`

## 🔐 Authentication

The API uses JWT Bearer authentication. To access protected endpoints:

1. Register a new user or login via the `/api/auth` endpoints
2. Copy the JWT token from the response
3. In Swagger UI, click "Authorize" and enter: `Bearer <your-token>`

## 📁 Project Structure

### Domain Layer
- **Entities**: Core business entities (User, Photographer, Portfolio, etc.)
- **Enums**: Domain-specific enumerations (UserRole, PortfolioCategory, etc.)
- **Exceptions**: Custom business exceptions

### Application Layer
- **Services**: Business logic implementation
- **DTOs**: Data transfer objects for API communication
- **Interfaces**: Service and repository contracts

### Infrastructure Layer
- **Data**: EF Core DbContext and configurations
- **Repositories**: Data access implementations
- **Migrations**: Database migration files

### API Layer
- **Controllers**: REST API endpoints
- **Filters**: Custom filters (e.g., file upload handling)
- **wwwroot/uploads**: Storage for uploaded portfolio images

### Client Layer
- **components**: React components
- **services**: API integration services
- **types**: TypeScript type definitions
- **utils**: Utility functions

## 🗄️ Database

The application uses SQLite for easy setup and portability. The database file (`photobooker.db`) is created automatically when you run migrations.

### Key Entities:
- **User**: Base user entity with authentication details
- **Client**: Client-specific profile extending User
- **Photographer**: Photographer profile with bio and specialization
- **Portfolio**: Collection of photography work
- **PortfolioImage**: Individual images within a portfolio

**Planned Entities** (for future booking system):
- **Package**: Service packages offered by photographers
- **BookingRequest**: Client booking requests
- **Shooting**: Confirmed photography sessions
- **Availability**: Photographer availability slots

## 🔧 Configuration

### Backend Configuration

Edit `PhotoBooker.API/appsettings.json` or `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=photobooker.db"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "PhotoBookerAPI",
    "Audience": "PhotoBookerClient"
  }
}
```

### Frontend Configuration

CORS is configured to allow the client at `http://localhost:5173`. Update `Program.cs` if using a different port.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is part of a university coursework.

## 👤 Author

**Karina Constandache**
- GitHub: [@karinaconstandache](https://github.com/karinaconstandache)

