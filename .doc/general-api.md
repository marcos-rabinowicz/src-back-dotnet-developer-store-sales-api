[Back to README](../README.md)

# General API Definitions

## Paginação
`_page` (1…), `_size` (10 por default)

## Ordenação
`_order="campo1 desc, campo2 asc"`

## Filtros
- `campo=valor` (igualdade)
- Strings com `*` prefixo/sufixo
- Range numérico/data: `_minPreco`, `_maxPreco`, `_minDate`

## Erros (formato)
```json
{ "type":"string", "error":"string", "detail":"string" }

Exemplos completos em Sales API.

<br> <div style="display: flex; justify-content: space-between;"> <a href="./frameworks.md">Previous: Frameworks</a> <a href="./products-api.md">Next: Products API (template)</a> </div> ```