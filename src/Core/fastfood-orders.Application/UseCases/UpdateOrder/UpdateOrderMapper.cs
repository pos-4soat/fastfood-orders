using AutoMapper;
using fastfood_orders.Domain.Entity;

namespace fastfood_orders.Application.UseCases.UpdateOrder;

public class UpdateOrderMapper : Profile
{
    public UpdateOrderMapper()
    {
        CreateMap<UpdateOrderRequest, OrderEntity>();
        CreateMap<OrderEntity, UpdateOrderResponse>();
    }
}
