# MG Data Storage Service

A .NET Web API implementing multi-layered data retrieval with caching, file storage, and database persistence.

## Overview

Data retrieval flow: **Cache (Redis) → File (JSON) → Database**
- Cache: 10 minutes TTL
- File: 30 minutes TTL
- Database: Persistent storage

## Tech Stack

- .NET 9.0
- Redis
- Docker

## Design Patterns

- Factory Pattern
- Chain of Responsibility
- Repository Pattern
- Dependency Injection

## Quick Start

### Prerequisites

- Docker & Docker Compose

### Setup

```bash
git clone <repository-url>
cd mg-data-storage-service
make demo
```

API: `http://localhost:5000` | Swagger: `http://localhost:5000/swagger`

### Commands

```bash
make demo    # Start everything
make stop    # Stop services
make test    # Run tests
make help    # Show all commands
```

## API

**GET /api/data/{id}**

Headers: `X-API-Key: <your-api-key>`

Response:
```json
{
  "id": "string",
  "data": {},
  "retrievedFrom": "Cache|File|Database"
}
```

## Security

- API Key authentication
- Input validation
- Rate limiting

## Configuration

Edit `.env`:
```env
REDIS_HOST=localhost
REDIS_PORT=6379
API_KEY=your-api-key
CACHE_DURATION_MINUTES=10
FILE_CACHE_DURATION_MINUTES=30
```

---

*Home assignment demonstrating clean architecture and design patterns*