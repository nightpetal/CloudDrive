# CloudDrive - Cloud Storage Application

<div align="center">

<div style="display: flex; flex-wrap: wrap; justify-content: center; gap: 8px;">

<a href="https://dotnet.microsoft.com/">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10.0">
</a>

<a href="https://react.dev/">
  <img src="https://img.shields.io/badge/React-18-61DAFB?style=for-the-badge&logo=react&logoColor=black" alt="React 18">
</a>

<a href="https://www.postgresql.org/">
  <img src="https://img.shields.io/badge/PostgreSQL-14%2B-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL 14+">
</a>

<a href="https://min.io/">
  <img src="https://img.shields.io/badge/MinIO-S3%20Storage-C72E29?style=for-the-badge&logo=minio&logoColor=white" alt="MinIO S3 Storage">
</a>

<a href="https://www.docker.com/">
  <img src="https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker&logoColor=white" alt="Docker Ready">
</a>

</div>

</div>

A modern, full-stack cloud storage application built with ASP .NET Core and React, featuring secure file uploads to MinIO, JWT authentication with refresh tokens, and PostgreSQL database persistence.

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Tech Stack](#tech-stack)
  - [Backend](#backend)
  - [Frontend](#frontend)
- [Installation](#installation)
  - [1. Backend Setup](#1-backend-setup)
  - [2. MinIO Setup](#2-minio-setup)
  - [3. Frontend Setup](#3-frontend-setup)
- [Project Structure](#project-structure)
- [Authentication Flow](#authentication-flow)
- [Database Schema](#database-schema)
  - [Users Table](#users-table)
  - [Files Table](#files-table)
  - [Folders Table](#folders-table)
  - [RefreshTokens Table](#refreshtokens-table)
- [API Endpoints](#api-endpoints)
  - [Authentication](#authentication)
  - [User Management](#user-management)
  - [File Management](#file-management)
  - [Folder Management](#folder-management)
- [Deployment](#deployment)
  - [Docker Deployment](#docker-deployment)
  - [Environment Variables](#environment-variables)
- [Documentation](#documentation)

## Features

- **User Authentication**: Secure login/registration with JWT tokens and refresh token support
- **File Management**: Upload, download, and delete files with real-time updates
- **Folder Organization**: Create and manage folders to organize your files
- **MinIO Integration**: Reliable object storage using MinIO S3-compatible storage
- **User Profiles**: View and manage user information with storage quota tracking
- **Responsive UI**: Modern, intuitive React frontend with Bootstrap styling
- **Role-Based Access**: Files are isolated per user with secure ownership validation

## Prerequisites

- **.NET 10.0** SDK
- **Node.js 18+** and npm
- **PostgreSQL 14+** database server
- **MinIO** object storage (Docker or standalone)
- **Docker** (optional, for containerized deployment)

## Tech Stack

### Backend

- **Framework**: ASP.NET Core 10.0
- **Database**: PostgreSQL with Entity Framework Core
- **Object Storage**: MinIO (S3-compatible)
- **Authentication**: JWT with refresh tokens
- **API Documentation**: OpenAPI/Scalar

### Frontend

- **Framework**: React 18
- **UI Library**: Bootstrap 5
- **HTTP Client**: Fetch API
- **Routing**: React Router
- **Icons**: React Icons

## Installation

### 1. Backend Setup

    cd CloudDrive.Backend/CloudDrive.API

    # Restore dependencies
    dotnet restore

    # Apply database migrations
    dotnet ef database update

    # Run the API
    dotnet run

The API will be available at `http://localhost:5214`

### 2. MinIO Setup

**Option A: Docker (Recommended)**

    docker run -p 9000:9000 -p 9001:9001 \
      -e MINIO_ROOT_USER=minioadmin \
      -e MINIO_ROOT_PASSWORD=minioadmin \
      minio/minio:latest server /data --console-address ":9001"

**Option B: Docker Compose**

    docker-compose -f compose.yaml up

MinIO Console: `http://localhost:9001`  
(credentials: `minioadmin/minioadmin`)

### 3. Frontend Setup

    cd CloudDrive.Frontend

    # Install dependencies
    npm install

    # Start development server
    npm run dev

The frontend will be available at `http://localhost:5173`

## Project Structure

    CloudDrive/
    ├── CloudDrive.Backend/
    │   ├── CloudDrive.API/              # ASP.NET Core Web API
    │   │   ├── Controllers/             # API endpoints
    │   │   ├── Program.cs               # Dependency injection & middleware config
    │   │   └── appsettings*.json        # Configuration files
    │   ├── CloudDrive.Application/      # Business logic layer
    │   │   ├── Services/                # Service implementations
    │   │   ├── Interfaces/              # Service contracts
    │   │   └── DTOs/                    # Data transfer objects
    │   ├── CloudDrive.Domain/           # Domain entities
    │   │   └── Entities/                # Core business models
    │   └── CloudDrive.Infrastructure/   # Data access layer
    │       ├── Repositories/            # Database access
    │       ├── Services/                # External service implementations (MinIO)
    │       └── Migrations/              # Database migrations
    ├── CloudDrive.Frontend/             # React application
    │   ├── src/
    │   │   ├── components/              # Reusable React components
    │   │   ├── pages/                   # Page components
    │   │   ├── services/                # API client services
    │   │   └── hooks/                   # Custom React hooks
    │   └── package.json
    ├── docs/                            # Documentation
    │   ├── ARCHITECTURE.md              # Architecture overview
    │   └── api.md                       # API documentation
    ├── README.md                        # This file
    └── compose.yaml                     # Docker Compose configuration

## Authentication Flow

1. User registers or logs in with email and password
2. Backend validates credentials and returns `accessToken` and `refreshToken`
3. Frontend stores both tokens in localStorage
4. Access token is included in Authorization header for API requests
5. When access token expires, refresh token is used to obtain a new one
6. Refresh token is stored for automatic token refresh

## Database Schema

### Users Table

- `Id` (GUID, Primary Key)
- `Email` (string, unique)
- `Username` (string)
- `PasswordHash` (string)
- `StorageLimitBytes` (int)
- `StorageUsed` (int)
- `JoinedDate` (DateTime)
- `Role` (string)

### Files Table

- `Id` (GUID, Primary Key)
- `OwnerId` (GUID, Foreign Key → Users)
- `FolderId` (GUID, Foreign Key → Folders, nullable)
- `OrginalName` (string)
- `StorageKey` (string) - MinIO object key
- `Extension` (string)
- `MimeType` (string)
- `SizeBytes` (int)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime, nullable)
- `DeletedAt` (DateTime, nullable)

### Folders Table

- `Id` (GUID, Primary Key)
- `OwnerId` (GUID, Foreign Key → Users)
- `ParentFolderId` (GUID, Foreign Key → Folders, nullable)
- `Name` (string)
- `CreatedAt` (DateTime)

### RefreshTokens Table

- `Id` (GUID, Primary Key)
- `UserId` (GUID, Foreign Key → Users)
- `Token` (string, unique)
- `ExpiryDate` (DateTime)
- `CreatedAt` (DateTime)

## API Endpoints

### Authentication

- `POST /login` - Login with email and password
- `POST /register` - Register new user
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/revoke` - Revoke refresh token

### User Management

- `GET /api/user/profile` - Get current user profile

### File Management

- `GET /api/files` - List user's files (paginated)
- `GET /api/files/{id}` - Get file details
- `POST /api/files/upload` - Upload a file
- `GET /api/files/download/{id}` - Download a file
- `DELETE /api/files/{id}` - Delete a file

### Folder Management

- `GET /api/folders` - List user's folders
- `POST /api/folders` - Create a new folder
- `DELETE /api/folders/{id}` - Delete a folder

## Deployment

### Docker Deployment

Build and run with Docker Compose:

    docker-compose -f compose.yaml up --build

### Environment Variables

Create `.env` file:

    ASPNETCORE_ENVIRONMENT=Production
    ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=clouddrive;Username=postgres;Password=yourpassword
    Minio__Endpoint=minio:9000
    Minio__AccessKey=minioadmin
    Minio__SecretKey=minioadmin
    Minio__UseSSL=false
    JWT__Key=your-secret-key-here

## Documentation

- [API Documentation](docs/api.md)
- [Architecture Overview](docs/ARCHITECTURE.md)
