# MG Data Storage Service

A .NET Web API implementing multi-layered data retrieval with caching, file storage, and database persistence.

## TL;DR

```bash
git clone <repository-url>
cd mg-data-storage
make demo                                    # Linux/macOS
# OR: setup-windows.bat                     # Windows
# API: http://localhost:5000
# Test: curl http://localhost:5000/data/user-001
```

**What it does:** Retrieves data from cache → file → database with fire-and-forget cache optimization.

## Overview

**Architecture:** Clean Architecture with Chain of Responsibility pattern for multi-layered data retrieval.

**Data Flow:**
```
API Request → Business Layer → Cache (Hybrid) → File (JSON) → Database (PostgreSQL)
                     ↓
              Fire-and-forget cache saves (async)
```

**Layers:**
- **API Layer** (`MG.DataStorage.API`) - Web API controllers and configuration  
- **Business Layer** (`MG.DataStorage.Business`) - Chain handlers and orchestration
- **Core Layer** (`MG.DataStorage.Core`) - Domain models, DTOs, and interfaces
- **Infrastructure Layer** (`MG.DataStorage.Infrastructure`) - Data access and external services

**Storage Strategy:**
- **Hybrid Cache**: In-Memory (50MB, 10min TTL) + Redis (unlimited, 10min TTL)
- **File Storage**: JSON persistence (30min TTL) 
- **Database**: PostgreSQL persistent storage
- **Optimization**: Fire-and-forget cache saves for improved response times

**Design Patterns:**
- **Factory Pattern** - Data provider creation (`IDataProviderFactory`)
- **Chain of Responsibility** - Cache → File → Database handlers
- **Repository Pattern** - Data access abstraction  
- **Dependency Injection** - Service registration and resolution

## Tech Stack

- .NET 9.0 Web API
- PostgreSQL 16 (database)
- Redis 8 (caching)
- Docker & Docker Compose

## Quick Start

### Prerequisites

- Docker & Docker Compose
- **Linux/macOS:** `make` (pre-installed)
- **Windows:** No additional tools required (`setup-windows.bat` included)

### Setup

**Linux/macOS:**
```bash
git clone <repository-url>
cd mg-data-storage
make demo
```

**Windows:**
```cmd
git clone <repository-url>
cd mg-data-storage
setup-windows.bat
```

API: `http://localhost:5000` | OpenAPI: `http://localhost:5000/openapi/v1.json`

### Commands

**Linux/macOS:**
```bash
make demo       # Start everything (containers only, no .NET required)
make demo-dev   # Start everything (requires .NET SDK)
make stop       # Stop services
make test       # Run tests
make help       # Show all commands
```

**Windows:**
```cmd
setup-windows.bat    # Full setup and start everything
docker-compose down  # Stop services
```

## API

**GET /data/{id}**

Sample request:
```bash
curl http://localhost:5000/data/user-001
```

Response:
```json
{
  "id": "user-001",
  "payload": {
    "name": "John Doe",
    "email": "john@example.com", 
    "role": "admin"
  },
  "retrievedFrom": "Cache"
}
```

**Sample Data IDs:**
- `user-001`, `user-002` (user profiles)
- `config-001` (application config)
- `product-001`, `product-002` (product catalog)

## Database

PostgreSQL with sample data is automatically set up via `make demo-full`.

**Schema:**
```sql
CREATE TABLE data_storage (
    id VARCHAR(255) PRIMARY KEY,
    content JSONB NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**Sample data:** 5 records (users, config, products) inserted automatically.

## Cache Strategy

**Hybrid Caching (In-Memory + Redis):**
1. **Read path:** Check in-memory → Check Redis → Fallback to file/database
2. **Write path:** Save to both in-memory and Redis (if memory not full)
3. **Fire-and-forget:** Cache saves happen asynchronously to improve response times

## Configuration

Edit `.env`:
```env
# Redis
REDIS_HOST=localhost
REDIS_PORT=6379

# PostgreSQL  
POSTGRESQL_CONNECTION_STRING=Host=localhost;Database=mg_data_storage;Username=postgres;Password=password

# Cache Settings
CACHE_DURATION_MINUTES=10
FILE_CACHE_DURATION_MINUTES=30

# API
API_PORT=5000
API_KEY=dev-api-key-change-in-production
```

## Testing

**Manual Testing:**
```bash
# Test with curl
curl http://localhost:5000/data/user-001

# Test cache behavior - first call hits database, second hits cache
curl -w "Time: %{time_total}s\n" http://localhost:5000/data/user-001
curl -w "Time: %{time_total}s\n" http://localhost:5000/data/user-001
```

**Postman Collection:**
Import `MG.DataStorage.postman_collection.json` for structured API testing with sample requests and responses.

## Troubleshooting

**Common issues:**
```bash
# Check if services are running
docker ps

# View logs
docker logs mg-postgres
docker logs mg-redis  
docker logs mg-api

# Reset everything
make stop && make clean && make demo-full

# Database connection issues
docker exec mg-postgres psql -U postgres -d mg_data_storage -c "SELECT COUNT(*) FROM data_storage;"
```

## Future Work

### Security (Recommended for API Gateway)
- **Authentication/Authorization** - JWT tokens, API keys (better handled by gateway)
- **Rate limiting** - Request throttling (gateway-level preferred)
- **CORS policy** - Cross-origin resource sharing (gateway responsibility)
- **Input validation** - Path traversal protection, SQL injection prevention

### Application-Level Improvements
- **Custom exception handling** - Use existing `DataNotFoundException`, `StorageException`
- **Comprehensive logging** - Structured logging with correlation IDs
- **Health checks** - Readiness/liveness probes for Kubernetes
- **Metrics** - Prometheus endpoints for monitoring
- **Circuit breaker** - Resilience patterns for external dependencies
- **Distributed tracing** - OpenTelemetry integration

### Operational
- **Unit/Integration tests** - Test coverage for all layers
- **Database migrations** - Schema versioning and upgrades
- **Configuration validation** - Startup configuration checks
- **Environment-specific configs** - Staging/production settings

---

*Home assignment demonstrating clean architecture and design patterns*