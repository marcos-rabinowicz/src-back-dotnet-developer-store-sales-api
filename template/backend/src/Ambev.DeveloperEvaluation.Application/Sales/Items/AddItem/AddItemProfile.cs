using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

public class AddItemProfile : Profile
{
    public AddItemProfile()
    {
        CreateMap<SaleItem, AddItemResult>()
            .ForCtorParam("ItemId", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("Quantity", opt => opt.MapFrom(s => s.Quantity))
            .ForCtorParam("UnitPrice", opt => opt.MapFrom(s => s.UnitPrice))
            .ForCtorParam("DiscountPercent", opt => opt.MapFrom(s => s.DiscountPercent))
            .ForCtorParam("LineTotal", opt => opt.MapFrom(s => s.LineTotal))
            .ForMember(dest => dest.SaleId, opt => opt.Ignore())
            .ForMember(dest => dest.SaleTotalAmount, opt => opt.Ignore());
    }
}
