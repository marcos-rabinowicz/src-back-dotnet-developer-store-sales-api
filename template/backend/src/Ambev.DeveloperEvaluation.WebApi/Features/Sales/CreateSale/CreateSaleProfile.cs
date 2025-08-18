using AutoMapper;
using App = Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleItemRequest, App.CreateSaleItemDto>();
        CreateMap<CreateSaleRequest, App.CreateSaleCommand>();

        CreateMap<App.CreateSaleItemResult, CreateSaleItemResponse>();
        CreateMap<App.CreateSaleResult, CreateSaleResponse>();
    }
}
