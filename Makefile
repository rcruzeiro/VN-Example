.PHONY: build
build: stop
	docker-compose build

.PHONY: up
up:
	docker-compose up -d

.PHONY: stop
stop:
	docker-compose stop

.PHONY: down
down:
	docker-compose down

.PHONY: test
test: up
	dotnet test src/VN.Example.Tests

.PHONY: migrate
migrate: up
	cd src/VN.Example.Infrastructure.Database.MSSQL && dotnet ef database update