using AutoMapper;
using fastfood_orders.Domain.Entity;

namespace fastfood_orders.Application.UseCases.CreateOrder
{
    public class CreateOrderMapper : Profile
    {
        public CreateOrderMapper()
        {
            CreateMap<CreateOrderRequest, OrderEntity>();
            CreateMap<OrderEntity, CreateOrderResponse>();
            CreateMap<OrderItens, OrderedItemEntity>();
        }
    }
}
