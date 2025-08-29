using Ambev.DeveloperEvaluation.Application.Orders.GetOrder;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.GetOrderFeature;

public class GetOrderMappingProfile : Profile
{
    public GetOrderMappingProfile()
    {
        CreateMap<GetOrderItemDto, GetOrderItemResponse>();
        CreateMap<GetOrderResult, GetOrderResponse>();
    }
}