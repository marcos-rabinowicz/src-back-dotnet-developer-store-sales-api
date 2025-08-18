using AutoMapper;
using App = Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.AddItem;

public class AddItemProfile : Profile
{
    public AddItemProfile()
    {
        CreateMap<AddItemRequest, App.AddItemCommand>();
        CreateMap<App.AddItemResult, AddItemResponse>();
    }
}
