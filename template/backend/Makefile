# Simple developer Makefile
SOL=Ambev.DeveloperEvaluation.sln
ORM=src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj
API=src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj
API_PORT?=5056

# database vars
DB_HOST?=localhost
DB_PORT?=5432
DB_USER?=dev
DB_PASS?=dev
DB_NAME?=developer_evaluation

export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development
export SalesRepository=Ef
export Sales__Repository=EfCore
export ConnectionStrings__DefaultConnection=Host=$(DB_HOST);Port=$(DB_PORT);Database=$(DB_NAME);Username=$(DB_USER);Password=$(DB_PASS);Include Error Detail=true
export ASPNETCORE_URLS=http://localhost:$(API_PORT)

.PHONY: build test pg-up migrate run postman pipeline clean-docker

build:
	dotnet clean $(SOL)
	dotnet restore $(SOL)
	dotnet build $(SOL) -c Debug -warnaserror

test:
	dotnet test $(SOL) --no-build --logger "trx;LogFileName=test-results.trx"

pg-up:
	docker rm -f ambev-pg >/dev/null 2>&1 || true
	docker run --name ambev-pg -e POSTGRES_USER=$(DB_USER) -e POSTGRES_PASSWORD=$(DB_PASS) -e POSTGRES_DB=$(DB_NAME) -p $(DB_PORT):5432 -d postgres:16
	sleep 6

migrate:
	dotnet ef database update -p $(ORM) -s $(API) --connection "$$ConnectionStrings__DefaultConnection"

run:
	dotnet run --no-build --project $(API)

postman:
	newman run .doc/ambev-sales-postman-collection.json --env-var baseUrl="http://localhost:$(API_PORT)" --reporters cli,junit --reporter-junit-export newman-report.xml

pipeline: build test pg-up migrate run postman

clean-docker:
	docker rm -f ambev-pg >/dev/null 2>&1 || true
