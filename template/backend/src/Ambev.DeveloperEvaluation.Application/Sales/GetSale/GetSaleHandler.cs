using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
{
    private readonly ISaleRepository _repo;
    private readonly IMapper _mapper;

    public GetSaleHandler(ISaleRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<GetSaleResult> Handle(GetSaleCommand request, CancellationToken ct)
    {
        GetSaleValidator.EnsureValid(request);

        var sale = await _repo.GetByIdAsync(request.Id, ct)
                   ?? throw new KeyNotFoundException("Sale not found.");

        return _mapper.Map<GetSaleResult>(sale);
    }
}
