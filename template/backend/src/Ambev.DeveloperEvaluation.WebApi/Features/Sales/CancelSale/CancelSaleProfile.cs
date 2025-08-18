using AutoMapper;
using App = Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public class CancelSaleProfile : Profile
{
    public CancelSaleProfile()
    {
        CreateMap<CancelSaleRequest, App.CancelSaleCommand>();
        CreateMap<App.CancelSaleResponse, CancelSaleResponse>();
    }
}
