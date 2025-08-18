using AutoMapper;
using App = Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.CancelItem;

public class CancelItemProfile : Profile
{
    public CancelItemProfile()
    {
        CreateMap<CancelItemRequest, App.CancelItemCommand>();
        CreateMap<App.CancelItemResponse, CancelItemResponse>();
    }
}
