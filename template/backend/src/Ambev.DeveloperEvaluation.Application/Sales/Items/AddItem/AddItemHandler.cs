using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

public class AddItemHandler : IRequestHandler<AddItemCommand, AddItemResult>
{
    private readonly ISaleRepository _repo;
    private readonly IMapper _mapper;

    public AddItemHandler(ISaleRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<AddItemResult> Handle(AddItemCommand request, CancellationToken ct)
    {
        AddItemValidator.EnsureValid(request);

        var sale = await _repo.GetByIdAsync(request.SaleId, ct)
                   ?? throw new KeyNotFoundException("Sale not found.");

        var item = sale.AddItem(
            ProductIdentity.Create(request.ProductId, request.ProductName),
            request.Quantity,
            request.UnitPrice);

        await _repo.UpdateAsync(sale, ct);

        var dto = _mapper.Map<AddItemResult>(item) with
        {
            SaleId = sale.Id,
            SaleTotalAmount = sale.TotalAmount
        };
        return dto;
    }
}
