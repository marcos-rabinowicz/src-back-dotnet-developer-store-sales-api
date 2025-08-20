[Back to README](../README.md)

# Sales API – Guia

API para **registrar vendas** e aplicar **regras de desconto por quantidade**.

## Regras de negócio
- 1–3 itens → **0%**
- 4–9 itens → **10%**
- 10–20 itens → **20%**
- **20 itens não é permitido** (erro de domínio)
- Cancelar item → zera o `lineTotal`;  
  Cancelar venda → nenhuma mutação posterior é permitida.

## Execução
- `dotnet run --project src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj`
- Swagger: `http://localhost:<porta>/swagger`
- Persistência: `"Sales:Repository": "InMemory"` (ou `"EfCore"`)

## Endpoints

### Criar venda
`POST /api/sales`

```json
{
  "saleNumber": "S-20250001",
  "date": "2025-01-01T12:00:00Z",
  "customerId": "00000000-0000-0000-0000-000000000001",
  "customerName": "Customer A",
  "branchId": "00000000-0000-0000-0000-000000000002",
  "branchName": "Branch RJ",
  "items": [
    { "productId":"00000000-0000-0000-0000-000000000101", "productName":"Beer A", "quantity":3,  "unitPrice":10 },
    { "productId":"00000000-0000-0000-0000-000000000102", "productName":"Beer B", "quantity":4,  "unitPrice":10 },
    { "productId":"00000000-0000-0000-0000-000000000103", "productName":"Beer C", "quantity":10, "unitPrice":10 }
  ]
}

201 Created – retorna a venda com totais e descontos calculados.


### Buscar venda

GET /api/sales/{id} → 200 OK


### Cancelar venda

POST /api/sales/{id}/cancel → 200 OK (status = Cancelled)


### Adicionar item

POST /api/sales/{id}/items → 200 OK

{ 
  "saleId":"...", 
  "productId":"...", 
  "productName":"Beer X", 
  "quantity":10, 
  "unitPrice":10 
}


### Alterar item

PUT /api/sales/{id}/items/{itemId} → 200 OK

Body: { 
  "saleId":"...", 
  "itemId":"...", 
  "quantity":4, 
  "unitPrice":15 
}


### Cancelar item

POST /api/sales/{id}/items/{itemId}/cancel → 200 OK


### Códigos de status

201 Created (criação de venda)

200 OK (demais operações)

400 Bad Request (validação de entrada)

404 Not Found (venda/item não localizado)

409 Conflict (violação de regra de domínio)

422 Unprocessable Entity (erros específicos de validação – quando middleware estiver habilitado)

Postman

Coleção: .doc/ambev-sales-postman-collection.json (variável {{baseUrl}})

REST Client (VS/VSCode): src/Ambev.DeveloperEvaluation.WebApi/Sales.http

<br> 
<div style="display: flex; justify-content: space-between;"> 
  <a href="./project-structure.md">Previous: Project Structure</a> 
  <a href="../README.md">Next: Read Me</a> 
</div> ```
