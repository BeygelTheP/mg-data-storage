# MG Data Storage - Makefile
# Automates setup and management of the multi-layered data storage service

.PHONY: help demo demo-dev stop test clean services db-init db-seed db-reset

# =============================================================================
# Configuration
# =============================================================================

# Database connection settings
DB_HOST=localhost
DB_PORT=5432
DB_NAME=mg_data_storage
DB_USER=postgres
DB_PASS=password

# =============================================================================
# Help & Main Targets
# =============================================================================

help: ## Show available commands
	@echo '🚀 MG Data Storage - Available Commands'
	@echo ''
	@echo 'Main Commands:'
	@grep -E '^(demo|demo-dev|stop|clean):.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-12s\033[0m %s\n", $$1, $$2}'
	@echo ''
	@echo 'Database Commands:'
	@grep -E '^(db-.*):.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-12s\033[0m %s\n", $$1, $$2}'
	@echo ''
	@echo 'Development Commands:'
	@grep -E '^(services|test):.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-12s\033[0m %s\n", $$1, $$2}'
	@echo ''

demo: ## 🚀 Start everything (full containerized setup)
	@cp -n .env.example .env 2>/dev/null || true
	@echo "🚀 Starting full demo environment..."
	@docker compose up -d
	@echo "⏳ Waiting for services to be ready..."
	@sleep 10
	@make --no-print-directory db-seed
	@echo "✅ Demo ready! API running at http://localhost:5000"
	@echo "🧪 Test: curl http://localhost:5000/data/user-001"

demo-dev: db-seed ## 🛠️  Development mode (requires .NET SDK)
	@echo "🚀 Starting API in development mode..."
	@cd src/MG.DataStorage.API && dotnet run

stop: ## 🛑 Stop all services
	@echo "🛑 Stopping services..."
	@docker compose down

clean: ## 🧹 Clean build artifacts and data
	@echo "🧹 Cleaning..."
	@find . -name "bin" -o -name "obj" | xargs rm -rf
	@rm -rf data/
	@echo "✨ Done!"

# =============================================================================
# Infrastructure Services
# =============================================================================

services: ## 🐳 Start database and cache services only
	@cp -n .env.example .env 2>/dev/null || true
	@echo "🐳 Starting database and cache services..."
	@docker compose up -d postgres redis
	@echo "⏳ Waiting for services to be ready..."
	@sleep 5

# =============================================================================
# Database Management
# =============================================================================

db-init: services ## 🗄️  Initialize database tables
	@echo "🗄️  Initializing database tables..."
	@docker cp sql/01-init-tables.sql mg-postgres:/tmp/01-init-tables.sql
	@docker exec mg-postgres psql -U $(DB_USER) -d $(DB_NAME) -f /tmp/01-init-tables.sql
	@echo "✅ Database tables initialized"

db-seed: db-init ## 🌱 Seed database with sample data
	@echo "🌱 Seeding database with sample data..."
	@docker cp sql/02-seed-data.sql mg-postgres:/tmp/02-seed-data.sql
	@docker exec mg-postgres psql -U $(DB_USER) -d $(DB_NAME) -f /tmp/02-seed-data.sql
	@echo "✅ Database seeded with sample data"

db-reset: ## 🔄 Reset database (drop and recreate)
	@echo "🔄 Resetting database..."
	@docker exec mg-postgres psql -U $(DB_USER) -d postgres -c "DROP DATABASE IF EXISTS $(DB_NAME);"
	@docker exec mg-postgres psql -U $(DB_USER) -d postgres -c "CREATE DATABASE $(DB_NAME);"
	@make --no-print-directory db-seed
	@echo "✅ Database reset complete"

# =============================================================================
# Development & Testing
# =============================================================================

test: ## 🧪 Run all tests
	@echo "🧪 Running tests..."
	@dotnet test

# =============================================================================
# Quick Access Targets (for convenience)
# =============================================================================

up: demo    ## Alias for demo
logs:       ## 📋 Show container logs
	@docker compose logs -f

status:     ## 📊 Show container status
	@docker ps --filter "name=mg-"