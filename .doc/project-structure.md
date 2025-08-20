[Back to README](../README.md)

# Project Structure

```text
root
├─ .doc/
│  ├─ overview.md
│  ├─ tech-stack.md
│  ├─ frameworks.md
│  ├─ general-api.md
│  ├─ sales-api.md             ← documentação principal da API
│  ├─ products-api.md          ← template (não implementado)
│  ├─ carts-api.md             ← template (não implementado)
│  ├─ users-api.md             ← template (não implementado)
│  └─ auth-api.md              ← template (não implementado)
│
├─ src/
│  ├─ Ambev.DeveloperEvaluation.Domain/
│  ├─ Ambev.DeveloperEvaluation.Application/
│  ├─ Ambev.DeveloperEvaluation.ORM/
│  ├─ Ambev.DeveloperEvaluation.IoC/
│  └─ Ambev.DeveloperEvaluation.WebApi/
│
├─ tests/
│  ├─ Ambev.DeveloperEvaluation.Unit/
│  ├─ Ambev.DeveloperEvaluation.Integration/
│  └─ Ambev.DeveloperEvaluation.Functional/
│
├─ docker-compose.yml (opcional)
└─ README.md


###Padrões

Domain: entidades/VOs, validações, specifications e eventos

Application: comandos/handlers (MediatR), profiles (AutoMapper), validações

ORM: DbContext, Migrations, Repositórios EF, repositório InMemory

IoC: módulos de registro (Application/Infrastructure/WebApi)

WebApi: Controllers & Middlewares enxutos

<br>
<div style="display: flex; justify-content: space-between;"> 
    <a href="./users-api.md">Previous: Users API (template)</a> 
    <a href="./sales-api.md">Next: Sales API</a> 
</div> ```