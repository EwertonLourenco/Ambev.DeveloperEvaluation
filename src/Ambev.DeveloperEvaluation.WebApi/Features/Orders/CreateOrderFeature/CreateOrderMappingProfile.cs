using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrderFeature;

public class CreateOrderMappingProfile : Profile
{
    public CreateOrderMappingProfile()
    {
        CreateMap<CreateOrderItemRequest, CreateOrderItemDto>();
        CreateMap<CreateOrderRequest, CreateOrderCommand>();

        CreateMap<CreateOrderResult, CreateOrderResponse>();
    }
}