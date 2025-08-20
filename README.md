# Ambev – Store Sales API

> Desafio back-end (.NET 8) com foco em **regras de negócio de Vendas (Sales)**, DDD simplificado e Clean Architecture. Controllers finos, casos de uso claros, testes unit/integration, e opção de persistência **InMemory** ou **PostgreSQL**.

## 🔗 Documentação

- Visão geral do desafio: [.doc/overview.md](.doc/overview.md)
- Tech Stack: [.doc/tech-stack.md](.doc/tech-stack.md)
- Frameworks: [.doc/frameworks.md](.doc/frameworks.md)
- Convenções de API (Paginação, Ordenação, Filtro, Erros): [.doc/general-api.md](.doc/general-api.md)
- **Sales API (principal)**: [.doc/sales-api.md](.doc/sales-api.md)
- Estrutura do projeto: [.doc/project-structure.md](.doc/project-structure.md)

> Os arquivos **Products / Carts / Users / Auth** estão no diretório `.doc/` como **material de template** (não implementado nesta solução), apenas para referência de estilo de documentação:
> [.doc/products-api.md](.doc/products-api.md) · [.doc/carts-api.md](.doc/carts-api.md) · [.doc/users-api.md](.doc/users-api.md) · [.doc/auth-api.md](.doc/auth-api.md)

---

## 🧭 Arquitetura

**Clean Architecture**  
`Domain → Application → Infrastructure (ORM/IoC) → WebApi`

- **Domain**: entidades, VOs, agregados, regras e *domain events*.
- **Application**: **use cases** (handlers com MediatR), DTOs/Profiles (AutoMapper), validações.
- **Infrastructure**: EF Core (PostgreSQL), repositórios, InMemory, IoC.
- **WebApi**: controllers finos → apenas orquestram use cases & validação.

Árvore completa: ver [.doc/project-structure.md](.doc/project-structure.md)

---

## ▶️ Como rodar

### 1) Requisitos
- .NET 8 SDK
- Docker (opcional, para subir PostgreSQL)
- Postman (opcional) – coleção em `.doc/ambev-sales-postman-collection.json`

### 2) Banco (opção A – Docker)
```bash
docker run --name dev-pg \
  -e POSTGRES_USER=dev -e POSTGRES_PASSWORD=dev \
  -e POSTGRES_DB=developer_evaluation \
  -p 5432:5432 -d postgres:16

### 3) Configuração
A WebApi lê appsettings.json + appsettings.Development.json.
Use Development para rodar local:

// src/Ambev.DeveloperEvaluation.WebApi/appsettings.Development.json
{
  "Sales": { "Repository": "InMemory" }, // ou "EfCore"
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=developer_evaluation;Username=dev;Password=dev;Include Error Detail=true"
  }
}

O DefaultContextFactory também permite dotnet ef fora de execução da API, com a mesma connection string.

### 4) Migrations (se usar PostgreSQL)
# gerar (já enviado no repo como exemplo)
```bash
dotnet ef migrations add Sales_Init \
  -c DefaultContext \
  -p src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj \
  -s src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj \
  -o Migrations
  
# aplicar
```bash
dotnet ef database update \
  -p src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj \
  -s src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj

5) Executar
```bash
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj
# Swagger: http://localhost:<porta>/swagger

🧪 Testes
# todos os testes
```bash
dotnet test

# por projeto
```bash
dotnet test tests/Ambev.DeveloperEvaluation.Unit/Ambev.DeveloperEvaluation.Unit.csproj
dotnet test tests/Ambev.DeveloperEvaluation.Integration/Ambev.DeveloperEvaluation.Integration.csproj
dotnet test tests/Ambev.DeveloperEvaluation.Functional/Ambev.DeveloperEvaluation.Functional.csproj

🧰 Padrões e ferramentas

DDD (entities/VOs/aggregates/specifications/repository)

Clean Architecture (separação de camadas)

MediatR (use cases), AutoMapper (mapeamentos)

EF Core (ORM), Npgsql

xUnit + FluentAssertions + Bogus (Faker) + NSubstitute

Serilog (logging), Swagger/OpenAPI

Mais detalhes: .doc/tech-stack.md
 e .doc/frameworks.md

📄 Licença

MIT