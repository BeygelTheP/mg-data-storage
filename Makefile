.PHONY: help demo stop test clean db-init db-seed db-reset services

# Database connection settings
DB_HOST=localhost
DB_PORT=5432
DB_NAME=mg_data_storage
DB_USER=postgres
DB_PASS=password

help: ## Show available commands
	@echo 'Usage: make [target]'
	@echo ''
	@echo 'Available targets:'
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-10s\033[0m %s\n", $$1, $$2}'

services: ## Start database and cache services only
	@cp -n .env.example .env 2>/dev/null || true
	@echo "🐳 Starting database and cache services..."
	@docker compose up -d postgres redis
	@echo "⏳ Waiting for services to be ready..."
	@sleep 5

db-init: services ## Initialize database tables
	@echo "🗄️  Initializing database tables..."
	@docker cp sql/01-init-tables.sql mg-postgres:/tmp/01-init-tables.sql
	@docker exec mg-postgres psql -U $(DB_USER) -d $(DB_NAME) -f /tmp/01-init-tables.sql
	@echo "✅ Database tables initialized"

db-seed: db-init ## Seed database with sample data
	@echo "🌱 Seeding database with sample data..."
	@docker cp sql/02-seed-data.sql mg-postgres:/tmp/02-seed-data.sql
	@docker exec mg-postgres psql -U $(DB_USER) -d $(DB_NAME) -f /tmp/02-seed-data.sql
	@echo "✅ Database seeded with sample data"

db-reset: ## Reset database (drop and recreate)
	@echo "🔄 Resetting database..."
	@docker exec mg-postgres psql -U $(DB_USER) -d postgres -c "DROP DATABASE IF EXISTS $(DB_NAME);"
	@docker exec mg-postgres psql -U $(DB_USER) -d postgres -c "CREATE DATABASE $(DB_NAME);"
	@make db-seed
	@echo "✅ Database reset complete"

demo: db-seed ## Start everything (database setup + services + API)
	@echo "🚀 Starting API..."
	@cd src/MG.DataStorage.API && dotnet run

stop: ## Stop all services
	@echo "🛑 Stopping services..."
	@docker compose down

test: ## Run all tests
	@dotnet test

clean: ## Clean build artifacts and data
	@echo "🧹 Cleaning..."
	@find . -name "bin" -o -name "obj" | xargs rm -rf
	@rm -rf data/
	@echo "✨ Done!"