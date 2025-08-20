# 🚀 Pipeline local (Build → Tests → Postgres → Migrations → WebApi → Postman)

Este guia mostra como executar, do zero, a pipeline local de desenvolvimento usando os scripts do repo.

> Pré-requisitos:
> - Docker (para subir o PostgreSQL local)
> - .NET 8 SDK
> - (Opcional) Node.js + **newman** (`npm i -g newman`) para rodar a coleção do Postman em CLI

---

## 1) Execução rápida

### Unix/WSL/macOS
```bash
chmod +x scripts/pipeline.sh
./scripts/pipeline.sh


Windows (PowerShell)
.\scripts\pipeline.ps1


O que os scripts fazem:

dotnet clean/restore/build (com -warnaserror para manter o código limpo)

dotnet test (unit + integration)

Sobe um container postgres:16 (ambev-pg) com: dev/dev@developer_evaluation

Aplica migrations do EF Core (forçando a ConnectionString)

Sobe a WebApi (padrão: http://localhost:5056)

(Opcional) Roda a coleção do Postman via newman


2) Variáveis de ambiente úteis

Você pode sobrescrever as variáveis antes de rodar o script:

| Variável   | Padrão                 |
| ---------- | ---------------------- |
| `DB_HOST`  | `localhost`            |
| `DB_PORT`  | `5432`                 |
| `DB_USER`  | `dev`                  |
| `DB_PASS`  | `dev`                  |
| `DB_NAME`  | `developer_evaluation` |
| `API_PORT` | `5056`                 |


A pipeline também força o uso do repositório EF no IoC (em vez de InMemory):

SalesRepository=Ef

Sales__Repository=EfCore

E injeta a ConnectionString para EF e WebApi via:

ConnectionStrings__DefaultConnection="Host=...;Port=...;Database=...;Username=...;Password=...;Include Error Detail=true"


3) Collection do Postman

Arquivo: .doc/ambev-sales-postman-collection.json

Se você tiver o newman instalado, o passo é automático.
Sem o newman, importe a coleção no Postman e use http://localhost:5056 como base.


4) Rodando por partes (Makefile)

Atalhos (se o Makefile estiver na raiz do repo):

make build      # clean+restore+build (warnaserror)
make test       # dotnet test
make pg-up      # sobe o Postgres em Docker
make migrate    # aplica migrations com a ConnectionString forçada
make run        # sobe a API (http://localhost:5056)
make postman    # roda a coleção do Postman (newman)
make pipeline   # build + test + pg-up + migrate + run + postman

5) Encerrando e limpando

Parar e remover o Postgres:

docker rm -f ambev-pg


Se rodou a API em segundo plano (Unix):

# veja o PID no log do script e finalize:
kill <PID>

6) Dicas e resolução de problemas

Porta ocupada (5432/5056): ajuste DB_PORT/API_PORT e reexecute.

EF não encontra connection: o script exporta ConnectionStrings__DefaultConnection. Se você preferir appsettings.Development.json, deixe valores coerentes.

JWT: garanta Jwt:SecretKey definido em appsettings.Development.json ou via env var.

Warnings virando erro: por padrão usamos -warnaserror. Se quiser relaxar localmente, rode o build com -warnaserror- (mas mantenha estrito no CI).

Logs da API: no Unix, o script escreve em /tmp/ambev-api.log.

7) CI (GitHub Actions)

Existe um workflow opcional em .github/workflows/ci.yml com build e testes.
A etapa de integração com Postgres (migrations + API + newman) está comentada/condicional – habilite quando desejar.