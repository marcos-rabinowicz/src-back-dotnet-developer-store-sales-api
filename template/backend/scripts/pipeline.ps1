Param(
  [string]$DbHost = "localhost",
  [int]$DbPort = 5432,
  [string]$DbUser = "dev",
  [string]$DbPass = "dev",
  [string]$DbName = "developer_evaluation",
  [int]$ApiPort = 5056,
  [string]$PgImage = "postgres:16"
)

$ErrorActionPreference = "Stop"

# Repositório (cobre ambos padrões)
$env:SalesRepository = "Ef"
$env:Sales__Repository = "EfCore"

# Ambiente
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:DOTNET_ENVIRONMENT = "Development"

# Connection string
$conn = "Host={0};Port={1};Database={2};Username={3};Password={4};Include Error Detail=true" -f $DbHost, $DbPort, $DbName, $DbUser, $DbPass
$env:ConnectionStrings__DefaultConnection = $conn

$solution = "Ambev.DeveloperEvaluation.sln"
$orm = "src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj"
$api = "src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"

Write-Host "=== 1) Clean/Restore/Build ==="
dotnet clean $solution
dotnet restore $solution
dotnet build $solution -c Debug -warnaserror

Write-Host "=== 2) Tests (Unit + Integration) ==="
dotnet test $solution --no-build --logger "trx;LogFileName=test-results.trx"

Write-Host "=== 3) Start PostgreSQL (Docker) ==="
docker rm -f ambev-pg *>$null
docker run --name ambev-pg -e POSTGRES_USER=$DbUser -e POSTGRES_PASSWORD=$DbPass -e POSTGRES_DB=$DbName -p $DbPort`:5432 -d $PgImage | Out-Null
Start-Sleep -Seconds 6

Write-Host "=== 4) Apply EF Core migrations ==="
dotnet ef database update -p $orm -s $api --connection "$conn"

Write-Host "=== 5) Run WebApi (bg) on :$ApiPort ==="
$env:ASPNETCORE_URLS = "http://localhost:$ApiPort"
$env:SalesRepository = "Ef"; $env:Sales__Repository = "EfCore"
Start-Process -FilePath "dotnet" -ArgumentList "run --no-build --project $api" -WindowStyle Hidden
Start-Sleep -Seconds 4

Write-Host "=== 6) Newman (Postman) ==="
$newman = Get-Command newman -ErrorAction SilentlyContinue
if ($null -ne $newman) {
  newman run ".doc/ambev-sales-postman-collection.json" --env-var "baseUrl=http://localhost:$ApiPort" --reporters cli,junit --reporter-junit-export newman-report.xml
} else {
  Write-Warning "Newman não encontrado; pulei este passo. Instale com: npm i -g newman"
}

Write-Host "=== DONE: local pipeline OK ==="
