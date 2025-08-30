using Ambev.DeveloperEvaluation.Application.Orders.ListOrders;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.ListOrdersFeature;

public class ListOrdersMappingProfile : Profile
{
    public ListOrdersMappingProfile()
    {
        CreateMap<ListOrdersRequest, ListOrdersQuery>();
        CreateMap<ListOrdersItem, ListOrdersItemResponse>();
        CreateMap<ListOrdersResult, ListOrdersResponse>();
    }
}