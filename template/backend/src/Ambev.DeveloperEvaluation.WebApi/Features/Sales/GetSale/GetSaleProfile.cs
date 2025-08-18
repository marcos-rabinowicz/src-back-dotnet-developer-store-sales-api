using AutoMapper;
using App = Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        CreateMap<App.GetSaleItemResult, GetSaleItemResponse>();
        CreateMap<App.GetSaleResult, GetSaleResponse>();
    }
}
