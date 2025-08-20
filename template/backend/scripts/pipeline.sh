#!/usr/bin/env bash
set -euo pipefail

# ===== Config (pode sobrescrever via env) =====
: "${DB_HOST:=localhost}"
: "${DB_PORT:=5432}"
: "${DB_USER:=dev}"
: "${DB_PASS:=dev}"
: "${DB_NAME:=developer_evaluation}"
: "${API_PORT:=5056}"
: "${PG_IMAGE:=postgres:16}"

# Repositório (cobre ambos padrões usados no template)
export SalesRepository="Ef"
export Sales__Repository="EfCore"

# Ambiente
export ASPNETCORE_ENVIRONMENT="Development"
export DOTNET_ENVIRONMENT="Development"

# Connection string (DefaultConnection) – serve para EF Tools e WebApi
export ConnectionStrings__DefaultConnection="Host=${DB_HOST};Port=${DB_PORT};Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASS};Include Error Detail=true"

SOLUTION="Ambev.DeveloperEvaluation.sln"
ORM="src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj"
API="src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"

echo "=== 1) Clean/Restore/Build ==="
dotnet clean "$SOLUTION"
dotnet restore "$SOLUTION"
dotnet build "$SOLUTION" -c Debug -warnaserror

echo "=== 2) Tests (Unit + Integration) ==="
dotnet test "$SOLUTION" --no-build --logger "trx;LogFileName=test-results.trx"

echo "=== 3) Start PostgreSQL (Docker) ==="
docker rm -f ambev-pg >/dev/null 2>&1 || true
docker run --name ambev-pg \
  -e POSTGRES_USER="${DB_USER}" \
  -e POSTGRES_PASSWORD="${DB_PASS}" \
  -e POSTGRES_DB="${DB_NAME}" \
  -p ${DB_PORT}:5432 -d "${PG_IMAGE}"
echo "Waiting Postgres..."; sleep 6

echo "=== 4) Apply EF Core migrations ==="
dotnet ef database update -p "$ORM" -s "$API" --connection "${ConnectionStrings__DefaultConnection}"

echo "=== 5) Run WebApi (bg) on :${API_PORT} ==="
export ASPNETCORE_URLS="http://localhost:${API_PORT}"
export SalesRepository="Ef"; export Sales__Repository="EfCore"
nohup dotnet run --no-build --project "$API" > /tmp/ambev-api.log 2>&1 &
API_PID=$!
echo "API PID=$API_PID (logs: /tmp/ambev-api.log)"; sleep 4

echo "=== 6) Newman (Postman collection) ==="
if command -v newman >/dev/null 2>&1; then
  newman run ".doc/ambev-sales-postman-collection.json" \
    --env-var baseUrl="http://localhost:${API_PORT}" \
    --reporters cli,junit --reporter-junit-export newman-report.xml
else
  echo "Newman não encontrado; pulei este passo. Instale com: npm i -g newman"
fi

echo "=== DONE: local pipeline OK ==="
echo "Para encerrar a API: kill ${API_PID}"
