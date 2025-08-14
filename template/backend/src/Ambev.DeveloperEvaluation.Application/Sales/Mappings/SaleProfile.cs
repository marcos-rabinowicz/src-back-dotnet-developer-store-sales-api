using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.Mappings;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<SaleItem, SaleItemResponseDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("ProductId", opt => opt.MapFrom(s => s.ProductId))
            .ForCtorParam("ProductNameSnapshot", opt => opt.MapFrom(s => s.ProductNameSnapshot))
            .ForCtorParam("Quantity", opt => opt.MapFrom(s => s.Quantity))
            .ForCtorParam("UnitPrice", opt => opt.MapFrom(s => s.UnitPrice))
            .ForCtorParam("DiscountPercent", opt => opt.MapFrom(s => s.DiscountPercent))
            .ForCtorParam("Total", opt => opt.MapFrom(s => s.Total));

        CreateMap<Sale, SaleResponseDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("SaleNumber", opt => opt.MapFrom(s => s.SaleNumber))
            .ForCtorParam("SaleDate", opt => opt.MapFrom(s => s.SaleDate))
            .ForCtorParam("CustomerId", opt => opt.MapFrom(s => s.CustomerId))
            .ForCtorParam("CustomerNameSnapshot", opt => opt.MapFrom(s => s.CustomerNameSnapshot))
            .ForCtorParam("BranchId", opt => opt.MapFrom(s => s.BranchId))
            .ForCtorParam("BranchNameSnapshot", opt => opt.MapFrom(s => s.BranchNameSnapshot))
            .ForCtorParam("Status", opt => opt.MapFrom(s => s.Status.ToString()))
            .ForCtorParam("TotalAmount", opt => opt.MapFrom(s => s.TotalAmount))
            .ForCtorParam("Items", opt => opt.MapFrom(s => s.Items));
    }
}
