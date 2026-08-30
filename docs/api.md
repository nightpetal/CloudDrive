# CloudDrive API Documentation

## Base URL

```
http://localhost:5214
```

## Authentication

All endpoints except `/login` and `/register` require JWT authentication.

**Header Format**:

```
Authorization: Bearer <accessToken>
```

## Response Format

### Success Response

```json
{
  "data": {
    /* response data */
  },
  "status": 200,
  "message": "Success"
}
```

### Error Response

```json
{
  "message": "Error description",
  "status": 400,
  "errors": {
    /* validation errors */
  }
}
```

## Authentication Endpoints

### Register User

Create a new user account.

```http
POST /register
Content-Type: application/json

{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "securePassword123"
}
```

**Response** (201 Created):

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIs..."
}
```

**Errors**:

- `400 Bad Request` - Invalid input
- `409 Conflict` - User already exists

---

### Login User

Authenticate with email and password.

```http
POST /login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "securePassword123"
}
```

**Response** (200 OK):

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIs..."
}
```

**Errors**:

- `401 Unauthorized` - Invalid credentials
- `400 Bad Request` - Missing required fields

---

### Refresh Token

Obtain a new access token using refresh token.

```http
POST /api/auth/refresh
Content-Type: application/json
Authorization: Bearer <refreshToken>

<refreshToken>
```

**Response** (200 OK):

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIs..."
}
```

**Errors**:

- `401 Unauthorized` - Invalid or expired refresh token

---

### Revoke Token

Invalidate a refresh token.

```http
POST /api/auth/revoke
Authorization: Bearer <accessToken>
Content-Type: application/json

<refreshToken>
```

**Response** (204 No Content)

**Errors**:

- `401 Unauthorized` - Not authenticated
- `400 Bad Request` - Invalid token

---

## User Endpoints

### Get User Profile

Retrieve current user's profile information.

```http
GET /api/user/profile
Authorization: Bearer <accessToken>
```

**Response** (200 OK):

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "email": "john@example.com",
  "username": "john_doe",
  "storageLimitBytes": 10737418240,
  "storageUsedBytes": 1073741824,
  "joinedDate": "2024-01-15T10:30:00Z"
}
```

**Errors**:

- `401 Unauthorized` - Invalid token
- `404 Not Found` - User not found

---

## File Endpoints

### List Files

Get paginated list of user's files.

```http
GET /api/files?page=1&pageSize=5&folderId=<optional-folder-id>
Authorization: Bearer <accessToken>
```

**Query Parameters**:

- `page` (int, default: 1) - Page number
- `pageSize` (int, default: 5) - Items per page
- `folderId` (guid, optional) - Filter by folder

**Response** (200 OK):

```json
{
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440001",
      "orginalName": "document.pdf",
      "extension": ".pdf",
      "sizeBytes": 1048576,
      "updatedAt": "2024-01-20T15:30:00Z"
    }
  ],
  "page": 1,
  "pageSize": 5,
  "hasNextPage": true
}
```

**Errors**:

- `401 Unauthorized` - Not authenticated

---

### Get File Details

Retrieve specific file information.

```http
GET /api/files/{fileId}
Authorization: Bearer <accessToken>
```

**Path Parameters**:

- `fileId` (guid) - File ID

**Response** (200 OK):

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440001",
  "orginalName": "document.pdf",
  "extension": ".pdf",
  "sizeBytes": 1048576,
  "mimeType": "application/pdf",
  "updatedAt": "2024-01-20T15:30:00Z"
}
```

**Errors**:

- `401 Unauthorized` - Not authenticated
- `404 Not Found` - File not found

---

### Upload File

Upload a file to user's storage.

```http
POST /api/files/upload?folderId=<optional-folder-id>
Authorization: Bearer <accessToken>
Content-Type: multipart/form-data

[binary file data]
```

**Query Parameters**:

- `folderId` (guid, optional) - Target folder ID

**Form Data**:

- `file` (binary) - The file to upload

**Response** (201 Created):

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440002",
  "orginalName": "image.jpg",
  "extension": ".jpg",
  "sizeBytes": 2097152,
  "mimeType": "image/jpeg",
  "storageKey": "550e8400-e29b-41d4-a716-446655440000/2024-01-20/abc123_image.jpg",
  "createdAt": "2024-01-20T16:45:00Z"
}
```

**Errors**:

- `400 Bad Request` - No file provided or file is empty
- `401 Unauthorized` - Not authenticated
- `413 Payload Too Large` - File exceeds size limit

---

### Download File

Download a file from storage.

```http
GET /api/files/download/{fileId}
Authorization: Bearer <accessToken>
```

**Path Parameters**:

- `fileId` (guid) - File ID to download

**Response** (200 OK):

- Content-Type: `[file's MIME type]`
- Content-Disposition: `attachment; filename="[filename]"`
- Body: Binary file data

**Errors**:

- `401 Unauthorized` - Not authenticated
- `404 Not Found` - File not found

---

### Delete File

Remove a file from storage.

```http
DELETE /api/files/{fileId}
Authorization: Bearer <accessToken>
```

**Path Parameters**:

- `fileId` (guid) - File ID to delete

**Response** (204 No Content)

**Errors**:

- `401 Unauthorized` - Not authenticated
- `404 Not Found` - File not found

---

## Folder Endpoints

### List Folders

Get user's folders.

```http
GET /api/folders?page=1&pageSize=10
Authorization: Bearer <accessToken>
```

**Query Parameters**:

- `page` (int, default: 1) - Page number
- `pageSize` (int, default: 10) - Items per page

**Response** (200 OK):

```json
{
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440010",
      "name": "Documents",
      "parentFolderId": null,
      "createdAt": "2024-01-10T12:00:00Z"
    }
  ],
  "page": 1,
  "pageSize": 10,
  "hasNextPage": false
}
```

**Errors**:

- `401 Unauthorized` - Not authenticated

---

### Create Folder

Create a new folder.

```http
POST /api/folders
Authorization: Bearer <accessToken>
Content-Type: application/json

{
  "name": "New Folder",
  "parentFolderId": null
}
```

**Request Body**:

- `name` (string, required) - Folder name
- `parentFolderId` (guid, optional) - Parent folder ID for nested structure

**Response** (201 Created):

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440011",
  "name": "New Folder",
  "parentFolderId": null,
  "createdAt": "2024-01-20T17:00:00Z"
}
```

**Errors**:

- `400 Bad Request` - Invalid input
- `401 Unauthorized` - Not authenticated

---

### Delete Folder

Remove a folder (must be empty).

```http
DELETE /api/folders/{folderId}
Authorization: Bearer <accessToken>
```

**Path Parameters**:

- `folderId` (guid) - Folder ID to delete

**Response** (204 No Content)

**Errors**:

- `401 Unauthorized` - Not authenticated
- `404 Not Found` - Folder not found
- `400 Bad Request` - Folder not empty

---

## Error Codes

| Code | Description                                            |
| ---- | ------------------------------------------------------ |
| 400  | Bad Request - Invalid input or missing required fields |
| 401  | Unauthorized - Missing or invalid authentication       |
| 403  | Forbidden - User lacks permission for this resource    |
| 404  | Not Found - Resource does not exist                    |
| 409  | Conflict - Resource already exists                     |
| 413  | Payload Too Large - File size exceeds limit            |
| 500  | Internal Server Error - Unexpected server error        |

---

## Rate Limiting

Currently not enforced. Production deployment should implement rate limiting per user/IP.

Recommended:

- 1000 requests per hour per user
- 100 MB max file size
- 10 GB per user storage limit

---

## Pagination

Paginated endpoints return:

- `data` - Array of items
- `page` - Current page number
- `pageSize` - Items per page
- `hasNextPage` - Whether more pages exist

Example:

```json
{
  "data": [...],
  "page": 1,
  "pageSize": 10,
  "hasNextPage": true
}
```

---

## Testing with cURL

### Register

```bash
curl -X POST http://localhost:5214/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "password123"
  }'
```

### Login

```bash
curl -X POST http://localhost:5214/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "password123"
  }'
```

### Get Profile

```bash
curl -X GET http://localhost:5214/api/user/profile \
  -H "Authorization: Bearer <accessToken>"
```

### Upload File

```bash
curl -X POST http://localhost:5214/api/files/upload \
  -H "Authorization: Bearer <accessToken>" \
  -F "file=@/path/to/file.txt"
```

### List Files

```bash
curl -X GET "http://localhost:5214/api/files?page=1&pageSize=5" \
  -H "Authorization: Bearer <accessToken>"
```

---

## Webhooks & Real-time Updates

Currently not implemented. Future enhancement to support:

- File upload completion
- File deletion
- Storage quota updates
- WebSocket for real-time collaboration

---

## API Versioning

Current API version: **v1** (implied in URL structure)

Future versions will use:

```
/api/v2/files
/api/v2/users
```

---

## Changelog

### v1.0.0 (Initial Release)

- Authentication (register, login, refresh token)
- File management (upload, download, delete, list)
- Folder management (create, delete, list)
- User profiles
- JWT-based security

---

## Support

For API issues or questions:

1. Check the documentation above
2. Review error messages carefully
3. Open an issue on GitHub repository
4. Contact development team
