using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.DTOs;

namespace Ambev.DeveloperEvaluation.Application.Sales.Services;

public interface ISaleService
{
    Task<SaleResponseDto> CreateAsync(CreateSaleRequestDto request, CancellationToken ct = default);
    Task<SaleResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<SaleResponseDto>> ListAsync(int page = 1, int pageSize = 20, CancellationToken ct = default);
    Task<SaleResponseDto> UpdateAsync(Guid id, CreateSaleRequestDto request, CancellationToken ct = default);
    Task<SaleResponseDto> CancelAsync(Guid id, CancellationToken ct = default);
    Task<SaleResponseDto> CancelItemAsync(Guid id, Guid itemId, CancellationToken ct = default);
}
