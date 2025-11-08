@echo off
echo 🚀 MG Data Storage - Windows Setup
echo.

REM Check if Docker is available
docker --version >nul 2>&1
if errorlevel 1 (
    echo ❌ Docker not found! Please install Docker Desktop for Windows.
    echo Download: https://www.docker.com/products/docker-desktop/
    pause
    exit /b 1
)

REM Copy environment file if it doesn't exist
if not exist .env (
    echo 📋 Copying environment configuration...
    copy .env.example .env >nul
)

REM Stop any existing containers
echo 🛑 Stopping any existing services...
docker-compose down >nul 2>&1

REM Start all services
echo 🐳 Starting database and cache services...
docker-compose up -d postgres redis
if errorlevel 1 (
    echo ❌ Failed to start services!
    pause
    exit /b 1
)

REM Wait for services to be ready
echo ⏳ Waiting for services to be ready...
timeout /t 10 /nobreak >nul

REM Initialize database
echo 🗄️ Initializing database tables...
docker cp sql/01-init-tables.sql mg-postgres:/tmp/01-init-tables.sql
if errorlevel 1 (
    echo ❌ Failed to copy init script!
    pause
    exit /b 1
)

docker exec mg-postgres psql -U postgres -d mg_data_storage -f /tmp/01-init-tables.sql
if errorlevel 1 (
    echo ❌ Failed to initialize database!
    pause
    exit /b 1
)

REM Seed database with sample data
echo 🌱 Seeding database with sample data...
docker cp sql/02-seed-data.sql mg-postgres:/tmp/02-seed-data.sql
docker exec mg-postgres psql -U postgres -d mg_data_storage -f /tmp/02-seed-data.sql
if errorlevel 1 (
    echo ❌ Failed to seed database!
    pause
    exit /b 1
)

REM Start API container
echo 🚀 Starting API...
docker-compose up -d api
if errorlevel 1 (
    echo ❌ Failed to start API!
    pause
    exit /b 1
)

echo.
echo ✅ Setup complete!
echo.
echo 🌐 API running at: http://localhost:5000
echo 📊 OpenAPI spec: http://localhost:5000/openapi/v1.json
echo.
echo 🧪 Test the API:
echo    curl http://localhost:5000/data/user-001
echo.
echo 📝 Available sample data:
echo    - user-001, user-002 (user profiles)
echo    - config-001 (application config)  
echo    - product-001, product-002 (product catalog)
echo.
echo 🛑 To stop services: docker-compose down
echo.
pause