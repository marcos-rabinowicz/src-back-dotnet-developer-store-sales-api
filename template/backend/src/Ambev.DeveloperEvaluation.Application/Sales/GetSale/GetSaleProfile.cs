using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        CreateMap<SaleItem, GetSaleItemResult>()
            .ForCtorParam("Id", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("ProductId", opt => opt.MapFrom(s => s.Product.Id))
            .ForCtorParam("ProductName", opt => opt.MapFrom(s => s.Product.Name))
            .ForCtorParam("Quantity", opt => opt.MapFrom(s => s.Quantity))
            .ForCtorParam("UnitPrice", opt => opt.MapFrom(s => s.UnitPrice))
            .ForCtorParam("DiscountPercent", opt => opt.MapFrom(s => s.DiscountPercent))
            .ForCtorParam("LineTotal", opt => opt.MapFrom(s => s.LineTotal));

        CreateMap<Sale, GetSaleResult>()
            .ForCtorParam("Id", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("SaleNumber", opt => opt.MapFrom(s => s.SaleNumber))
            .ForCtorParam("Date", opt => opt.MapFrom(s => s.Date))
            .ForCtorParam("CustomerId", opt => opt.MapFrom(s => s.Customer.Id))
            .ForCtorParam("CustomerName", opt => opt.MapFrom(s => s.Customer.Name))
            .ForCtorParam("BranchId", opt => opt.MapFrom(s => s.Branch.Id))
            .ForCtorParam("BranchName", opt => opt.MapFrom(s => s.Branch.Name))
            .ForCtorParam("TotalAmount", opt => opt.MapFrom(s => s.TotalAmount))
            .ForCtorParam("Status", opt => opt.MapFrom(s => s.Status.ToString()))
            .ForCtorParam("Items", opt => opt.MapFrom(s => s.Items));
    }
}
