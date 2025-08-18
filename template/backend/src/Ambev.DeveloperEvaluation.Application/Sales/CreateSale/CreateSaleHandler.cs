using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _repo;
    private readonly IMapper _mapper;

    public CreateSaleHandler(ISaleRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken ct)
    {
        CreateSaleValidator.EnsureValid(request);

        var sale = new Sale(
            request.SaleNumber,
            request.Date,
            CustomerIdentity.Create(request.CustomerId, request.CustomerName),
            BranchIdentity.Create(request.BranchId, request.BranchName));

        foreach (var i in request.Items)
        {
            sale.AddItem(
                ProductIdentity.Create(i.ProductId, i.ProductName),
                i.Quantity,
                i.UnitPrice);
        }

        await _repo.AddAsync(sale, ct);

        return _mapper.Map<CreateSaleResult>(sale);
    }
}
