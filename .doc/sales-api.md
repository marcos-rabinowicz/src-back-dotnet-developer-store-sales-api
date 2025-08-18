Sales API – Guia Rápido
Visão geral

API para registrar vendas e aplicar regras de negócio de desconto por quantidade.

Regras:

1–3 itens: 0%

4–9 itens: 10%

10–20 itens: 20%

20 itens não permitido (erro de domínio)

Cancelar item zera o total da linha; cancelar a venda bloqueia qualquer mutação.

Executando
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj


Swagger: http://localhost:<porta>/swagger

Repositório de vendas: InMemory por padrão
(appsettings: "SalesRepository": "InMemory")

Endpoints
Criar venda

POST /api/sales

{
  "saleNumber": "S-20250001",
  "date": "2025-01-01T12:00:00Z",
  "customerId": "9b5d5a4e-0b0f-4a6d-8a72-4e2f2a7bb001",
  "customerName": "Customer A",
  "branchId": "d62f7a6a-1b79-408b-a2d2-5644c6b20001",
  "branchName": "Branch RJ",
  "items": [
    { "productId": "e1f1...", "productName": "Beer A", "quantity": 3,  "unitPrice": 10 },
    { "productId": "e1f2...", "productName": "Beer B", "quantity": 4,  "unitPrice": 10 },
    { "productId": "e1f3...", "productName": "Beer C", "quantity": 10, "unitPrice": 10 }
  ]
}


201 Created

{
  "id": "c0f0...",
  "saleNumber": "S-20250001",
  "date": "2025-01-01T12:00:00Z",
  "customerId": "9b5d5a4e-...",
  "customerName": "Customer A",
  "branchId": "d62f7a6a-...",
  "branchName": "Branch RJ",
  "totalAmount": 146.00,
  "status": "Active",
  "items": [
    { "id":"...", "productId":"...", "productName":"Beer A", "quantity":3,  "unitPrice":10, "discountPercent":0.00, "lineTotal":30.00 },
    { "id":"...", "productId":"...", "productName":"Beer B", "quantity":4,  "unitPrice":10, "discountPercent":0.10, "lineTotal":36.00 },
    { "id":"...", "productId":"...", "productName":"Beer C", "quantity":10, "unitPrice":10, "discountPercent":0.20, "lineTotal":80.00 }
  ]
}

Buscar venda

GET /api/sales/{id}
200 OK → corpo igual ao de criação (com totais atualizados)

Cancelar venda

POST /api/sales/{id}/cancel
200 OK

{ "id": "c0f0...", "status": "Cancelled" }

Adicionar item

POST /api/sales/{id}/items

{
  "saleId": "c0f0...",
  "productId": "77b2...",
  "productName": "Beer X",
  "quantity": 10,
  "unitPrice": 10
}


200 OK

{
  "saleId":"c0f0...","itemId":"ab12...",
  "quantity":10,"unitPrice":10,"discountPercent":0.20,"lineTotal":80.00,
  "saleTotalAmount":226.00
}

Alterar item

PUT /api/sales/{id}/items/{itemId}

{ "saleId":"c0f0...", "itemId":"ab12...", "quantity":4, "unitPrice":15 }


200 OK

{ "saleId":"c0f0...", "itemId":"ab12...", "saleTotalAmount":200.00 }

Cancelar item

POST /api/sales/{id}/items/{itemId}/cancel
200 OK

{ "saleId":"c0f0...", "itemId":"ab12...", "saleTotalAmount":146.00 }

Erros comuns

400 Bad Request: validação de entrada (ids vazios, quantity ≤ 0, unitPrice < 0).

404 Not Found: venda não localizada.

409 Conflict: regra de domínio violada (ex.: quantity > 20, alterar/cancelar em venda cancelada).

422 Unprocessable Entity: quando o middleware de validação do template retornar problemas específicos de validação (se configurado).

Testes rápidos (cURL)
# Criar venda
curl -s -X POST http://localhost:5000/api/sales \
  -H "Content-Type: application/json" \
  -d @- <<'JSON'
{ "saleNumber":"S-1","date":"2025-01-01T12:00:00Z",
  "customerId":"00000000-0000-0000-0000-000000000001","customerName":"Customer A",
  "branchId":"00000000-0000-0000-0000-000000000002","branchName":"Branch RJ",
  "items":[
    {"productId":"00000000-0000-0000-0000-000000000101","productName":"Beer A","quantity":3,"unitPrice":10},
    {"productId":"00000000-0000-0000-0000-000000000102","productName":"Beer B","quantity":4,"unitPrice":10},
    {"productId":"00000000-0000-0000-0000-000000000103","productName":"Beer C","quantity":10,"unitPrice":10}
  ] }
JSON

Swagger

Disponível em /swagger.

Endpoints de Sales aparecem automaticamente (perfis/validators mapeados via AutoMapper/MediatR).

Postman / .http

Postman: importe a coleção “Ambev Store Sales API” e ajuste {{baseUrl}}.

VS/VSCode REST Client: use src/Ambev.DeveloperEvaluation.WebApi/Sales.http.

Switch de persistência

Por ora: "SalesRepository": "InMemory".

Quando houver EfSaleRepository, basta mudar o valor no appsettings (sem alterar código).