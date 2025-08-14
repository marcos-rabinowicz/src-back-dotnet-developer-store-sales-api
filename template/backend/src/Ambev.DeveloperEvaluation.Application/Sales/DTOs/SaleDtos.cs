using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs;

public record ExternalRefDto(string Id, string NameSnapshot);

public record CreateSaleItemDto(
    string ProductId, string ProductNameSnapshot, int Quantity, decimal UnitPrice);

public record CreateSaleRequestDto(
    string SaleNumber,
    DateTime SaleDate,
    ExternalRefDto Customer,
    ExternalRefDto Branch,
    List<CreateSaleItemDto> Items);

public record SaleItemResponseDto(
    Guid Id, string ProductId, string ProductNameSnapshot, int Quantity, decimal UnitPrice, decimal DiscountPercent, decimal Total);

public record SaleResponseDto(
    Guid Id,
    string SaleNumber,
    DateTime SaleDate,
    string CustomerId,
    string CustomerNameSnapshot,
    string BranchId,
    string BranchNameSnapshot,
    string Status,
    decimal TotalAmount,
    List<SaleItemResponseDto> Items);
