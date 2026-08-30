# CloudDrive Architecture

## System Overview

CloudDrive is a modern cloud storage application built following a layered architecture pattern with clear separation of concerns. The system consists of a backend API (ASP.NET Core) and a frontend application (React), communicating through RESTful APIs.

```
┌─────────────────────────────────────────────────────────────────┐
│                     React Frontend (Port 5173)                  │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
│  │   Pages      │  │  Components  │  │    Hooks     │           │
│  │   (Upload,   │  │  (UserFile,  │  │  (SetTitle)  │           │
│  │   Profile,   │  │   UserFolder)│  │              │           │
│  │   Drive)     │  │              │  │              │           │
│  └──────────────┘  └──────────────┘  └──────────────┘           │
│         ↓                  ↓                   ↓                 │
│  ┌──────────────────────────────────────────────────────┐       │
│  │            API Services Layer                        │       │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐ │       │
│  │  │  authAPI.js │  │  fileAPI.js │  │ userAPI.js  │ │       │
│  │  └─────────────┘  └─────────────┘  └─────────────┘ │       │
│  └──────────────────────────────────────────────────────┘       │
│                              ↓                                  │
├─────────────────────────────────────────────────────────────────┤
│         HTTP/HTTPS (REST API on Port 5214)                      │
├─────────────────────────────────────────────────────────────────┤
│              ASP.NET Core API (Port 5214)                       │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Controllers Layer (API Endpoints)                        │  │
│  │ ┌──────────────┐ ┌──────────────┐ ┌──────────────┐      │  │
│  │ │AuthController│ │FileController│ │UserController      │  │
│  │ └──────────────┘ └──────────────┘ └──────────────┘      │  │
│  └──────────────────────────────────────────────────────────┘  │
│                              ↓                                  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Application Layer (Services & DTOs)                      │  │
│  │ ┌─────────────┐  ┌─────────────┐  ┌─────────────┐        │  │
│  │ │FileService  │  │AuthService  │  │UserService  │        │  │
│  │ └─────────────┘  └─────────────┘  └─────────────┘        │  │
│  └──────────────────────────────────────────────────────────┘  │
│                              ↓                                  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Infrastructure Layer (Repositories & Data Access)        │  │
│  │ ┌──────────────┐  ┌──────────────┐  ┌──────────────┐     │  │
│  │ │FileRepository│  │UserRepository│  │FolderRepository     │  │
│  │ │  (Database)  │  │  (Database)  │  │  (Database)  │     │  │
│  │ └──────────────┘  └──────────────┘  └──────────────┘     │  │
│  │ ┌──────────────────────────────────────────────────────┐ │  │
│  │ │MinioStorageService (Object Storage)                  │ │  │
│  │ └──────────────────────────────────────────────────────┘ │  │
│  └──────────────────────────────────────────────────────────┘  │
│                              ↓                                  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Domain Layer (Core Entities)                             │  │
│  │ ┌──────────────┐  ┌──────────────┐  ┌──────────────┐     │  │
│  │ │User Entity   │  │File Entity   │  │Folder Entity │     │  │
│  │ └──────────────┘  └──────────────┘  └──────────────┘     │  │
│  └──────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
         ↓                                          ↓
    ┌──────────────────┐                  ┌──────────────────┐
    │  PostgreSQL      │                  │  MinIO S3        │
    │  Database        │                  │  Object Storage  │
    │  (Metadata)      │                  │  (Files)         │
    └──────────────────┘                  └──────────────────┘
```

## Architecture Layers

### 1. **Presentation Layer** (Frontend - React)

**Responsibility**: Handle user interface and user interactions.

**Components**:

- **Pages**: `LoginPage`, `RegisterPage`, `DrivePage`, `ProfilePage`, `HomePage`, `AboutPage`
- **Components**: `Navbar`, `Sidebar`, `UserFile`, `UserFolder`, `ProfileCard`, `Footer`
- **Services**: API client wrappers (`authAPI.js`, `fileAPI.js`, `userAPI.js`, `apiCall.js`)
- **Hooks**: Custom React hooks like `SetTitle` for page management

**Technologies**:

- React 18
- React Router for navigation
- Bootstrap 5 for styling
- React Icons for icons
- Fetch API for HTTP requests

**Data Flow**:

1. User interacts with UI
2. Event handlers call API services
3. API services send HTTP requests to backend
4. Response is processed and state is updated
5. Component re-renders with new data

### 2. **API/Controller Layer** (ASP.NET Core)

**Responsibility**: Handle HTTP requests, validate input, and orchestrate responses.

**Endpoints**:

#### Authentication Controller

- `POST /login` - User login
- `POST /register` - User registration
- `POST /api/auth/refresh` - Token refresh
- `POST /api/auth/revoke` - Token revocation

#### File Controller

- `GET /api/files` - List files with pagination
- `GET /api/files/{id}` - Get file details
- `POST /api/files/upload` - Upload file
- `GET /api/files/download/{id}` - Download file
- `DELETE /api/files/{id}` - Delete file

#### User Controller

- `GET /api/user/profile` - Get user profile

#### Folder Controller

- `GET /api/folders` - List folders
- `POST /api/folders` - Create folder
- `DELETE /api/folders/{id}` - Delete folder

**Features**:

- JWT authentication with `[Authorize]` attribute
- Request validation
- Error handling and appropriate HTTP status codes
- CORS support for frontend
- OpenAPI/Swagger documentation

### 3. **Application/Business Logic Layer**

**Responsibility**: Implement business rules, data transformation, and orchestration.

**Services**:

#### FileService

- `UploadFileAsync()` - Handle file upload to MinIO and database
- `DownloadFileAsync()` - Retrieve file from MinIO
- `DeleteFileAsync()` - Remove file from MinIO and database
- `UpdateFileAsync()` - Update file metadata

#### AuthService

- `Login()` - Authenticate user and generate tokens
- `Register()` - Create new user account
- `RefreshToken()` - Generate new access token
- `RevokeRefreshToken()` - Invalidate refresh token

#### UserService

- User profile management
- Storage quota tracking

#### FolderService

- Folder creation and management
- Folder hierarchy maintenance

**DTOs** (Data Transfer Objects):

- `LoginRequest`, `RegisterRequest` - Input validation
- `AuthResponse` - Token response
- `UserProfileDto` - User data transport
- `FileRequest`, `FolderRequest` - File/folder data transport
- `AddFileDto`, `UpdateFileDto` - File operation requests

### 4. **Infrastructure/Data Access Layer**

**Responsibility**: Handle database operations and external service integrations.

**Repositories** (Data Access Objects):

- `IUserRepository` - User data access
- `IFileRepository` - File metadata access
- `IFolderRepository` - Folder data access
- `IRefreshTokenRepository` - Token management

**Services**:

- `MinioStorageService` - Object storage operations
  - `UploadFileAsync()` - Upload to MinIO bucket
  - `DownloadFileAsync()` - Retrieve from MinIO
  - `DeleteFileAsync()` - Remove from MinIO
  - `BucketExistsAsync()` - Check/create bucket

**Database Access**:

- Entity Framework Core for ORM
- PostgreSQL as relational database
- Database migrations for schema management

### 5. **Domain Layer**

**Responsibility**: Define core business entities and domain rules.

**Entities**:

- `User` - User account with authentication and storage info
- `File` - File metadata (name, size, MIME type, storage key)
- `Folder` - Folder structure for file organization
- `RefreshToken` - Token storage for session management

**Characteristics**:

- No dependencies on other layers
- Pure domain logic
- Used across all other layers

## Data Flow Diagrams

### File Upload Flow

```
Frontend                    Backend                  MinIO
   │                           │                       │
   ├─ Select file ─────────────→│                       │
   │                           │                       │
   ├─ POST /api/files/upload ──→│                       │
   │  (multipart/form-data)    │                       │
   │                           │ UploadFileAsync()     │
   │                           ├──────────────────────→│
   │                           │ Upload to bucket      │
   │                           │←──────────────────────┤
   │                           │ Success               │
   │                           │ Create DB record      │
   │                           │ Save metadata         │
   │←─────── 201 Created ───────┤                       │
   │ (File object)             │                       │
   │                           │                       │
   └─ Update file list         │                       │
```

### User Login Flow

```
Frontend                Backend                   DB
   │                       │                      │
   ├─ POST /login ────────→│                      │
   │ (email, password)    │                      │
   │                       ├─ Get user ─────────→│
   │                       │←─ User object ──────┤
   │                       │ Hash password       │
   │                       │ Validate            │
   │                       │ Generate JWT        │
   │                       │ Generate RefreshToken
   │                       │ Save token ────────→│
   │                       │←─ OK ──────────────┤
   │←─ 200 OK ────────────┤                      │
   │ { accessToken,       │                      │
   │   refreshToken }     │                      │
   │ Store in localStorage│                      │
   │                       │                      │
```

### File Download Flow

```
Frontend                Backend               MinIO
   │                       │                   │
   ├─ GET /api/files/download/{id} ──────────→│
   │                       │                   │
   │                       ├─ Get file metadata│
   │                       │ (storageKey)      │
   │                       │ DownloadFileAsync()
   │                       ├──────────────────→│
   │                       │ Get file stream   │
   │                       │←──────────────────┤
   │←─ File (binary) ──────┤ (buffered)        │
   │ Content-Type header   │                   │
   │ Browser triggers      │                   │
   │ download              │                   │
```

## Authentication & Security

### JWT Implementation

1. **Access Token**
   - Short-lived (15 minutes by default)
   - Contains user claims (ID, email, role)
   - Included in every API request header
   - Signed with secret key

2. **Refresh Token**
   - Long-lived (7 days by default)
   - Stored in database for validation
   - Stored in localStorage on client
   - Used to obtain new access tokens
   - Can be revoked

### Security Features

- **Password Hashing**: Bcrypt for password storage
- **JWT Validation**: Token signature verification on every request
- **CORS**: Cross-Origin Resource Sharing restricted to frontend origin
- **HTTPS**: Recommended for production
- **Owner Validation**: All file/folder operations validate user ownership
- **Input Validation**: DTO validation on all API endpoints

## Storage Architecture

### Database (PostgreSQL)

Stores:

- User accounts and authentication data
- File metadata (name, size, MIME type, storage key)
- Folder structure
- Refresh tokens
- Timestamps and relationships

```sql
Users ──┬─→ Files
        ├─→ Folders
        └─→ RefreshTokens
```

### Object Storage (MinIO)

Stores:

- Actual file contents (binary data)
- Organized by user and date in bucket path structure
- Path format: `{userId}/{date}/{guid}_{filename}`

**Benefits**:

- Scalability for large files
- Separate from database
- Fast retrieval
- Cost-effective for large storage volumes

## Error Handling

### Backend Error Handling

1. **Controllers**: Catch exceptions and return appropriate HTTP status codes
   - `400 Bad Request` - Validation errors
   - `401 Unauthorized` - Authentication required
   - `403 Forbidden` - Insufficient permissions
   - `404 Not Found` - Resource not found
   - `500 Internal Server Error` - Unexpected errors

2. **Services**: Throw domain-specific exceptions
   - `InvalidOperationException` - Business logic violation
   - `UnauthorizedAccessException` - Permission denied

### Frontend Error Handling

1. **API Calls**: Catch fetch errors and HTTP errors
2. **User Feedback**: Display error alerts to user
3. **State Management**: Update UI with error messages
4. **Logging**: Console logging for debugging

## Deployment Considerations

### Development Environment

- Local PostgreSQL instance
- MinIO running on localhost:9000
- Frontend on localhost:5173
- Backend on localhost:5214

### Production Environment

- Managed PostgreSQL (AWS RDS, Azure Database, etc.)
- MinIO cluster or S3-compatible storage (AWS S3, DigitalOcean Spaces)
- Frontend deployed on CDN (Vercel, Netlify, CloudFlare)
- Backend on application server (Heroku, AWS, Azure, Digital Ocean)
- HTTPS enforced
- Environment variables for sensitive configuration

### Docker Deployment

- Backend and MinIO containers orchestrated with Docker Compose
- PostgreSQL container for database
- Shared volumes for persistence
- Network isolation between services

## Scalability Patterns

### Horizontal Scaling

1. **API Layer**: Multiple backend instances behind load balancer
2. **Database**: Connection pooling, read replicas
3. **MinIO**: Clustering for distributed storage

### Caching

- Frontend: Browser cache, localStorage for tokens
- Backend: In-memory caching for frequently accessed data
- CDN: Caching static assets

### Database Optimization

- Indexes on frequently queried columns (UserId, Email)
- Connection pooling
- Query optimization with Entity Framework projections

## Technology Stack Rationale

| Component      | Technology      | Reason                                          |
| -------------- | --------------- | ----------------------------------------------- |
| Backend        | ASP.NET Core 10 | Enterprise-grade, high performance, built-in DI |
| Database       | PostgreSQL      | Reliable RDBMS, good scalability, open-source   |
| Object Storage | MinIO           | S3-compatible, self-hosted alternative          |
| Frontend       | React 18        | Modern, component-based, large ecosystem        |
| Authentication | JWT             | Stateless, scalable, industry standard          |
| API Client     | Fetch API       | Built-in browser API, no extra dependencies     |
| Styling        | Bootstrap 5     | Responsive, extensive components, accessibility |

## Future Architecture Improvements

1. **Caching Layer**: Redis for session and token caching
2. **Message Queue**: RabbitMQ/Kafka for async operations
3. **Microservices**: Split into separate services (Auth, Files, Storage)
4. **GraphQL**: Alternative to REST API for complex queries
5. **Real-time Updates**: WebSocket support for live file updates
6. **Search**: Elasticsearch integration for full-text search
7. **Monitoring**: Application Performance Monitoring (APM)
8. **Logging**: Centralized logging system (ELK stack)

---

## References

- [ASP.NET Core Architecture](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [JWT Best Practices](https://tools.ietf.org/html/rfc8725)
- [React Best Practices](https://react.dev/)
