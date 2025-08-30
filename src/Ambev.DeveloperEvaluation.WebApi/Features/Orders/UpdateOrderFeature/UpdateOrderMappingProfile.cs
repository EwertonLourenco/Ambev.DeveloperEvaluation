using Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrderFeature;

public class UpdateOrderMappingProfile : Profile
{
    public UpdateOrderMappingProfile()
    {
        CreateMap<UpdateOrderItemRequest, UpdateOrderItemDto>();
        CreateMap<UpdateOrderRequest, UpdateOrderCommand>();
        CreateMap<UpdateOrderResult, UpdateOrderResponse>();
    }
}