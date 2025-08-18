using AutoMapper;
using App = Ambev.DeveloperEvaluation.Application.Sales.Items.ChangeItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.ChangeItem;

public class ChangeItemProfile : Profile
{
    public ChangeItemProfile()
    {
        CreateMap<ChangeItemRequest, App.ChangeItemCommand>();
        CreateMap<App.ChangeItemResponse, ChangeItemResponse>();
    }
}
