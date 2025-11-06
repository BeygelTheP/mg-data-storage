.PHONY: help demo stop test clean

help: ## Show available commands
	@echo 'Usage: make [target]'
	@echo ''
	@echo 'Available targets:'
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-10s\033[0m %s\n", $$1, $$2}'

demo: ## Start everything (setup + services + API)
	@cp -n .env.example .env 2>/dev/null || true
	@echo "🐳 Starting Docker services..."
	@docker compose up -d
	@echo "⏳ Waiting for services..."
	@sleep 3
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