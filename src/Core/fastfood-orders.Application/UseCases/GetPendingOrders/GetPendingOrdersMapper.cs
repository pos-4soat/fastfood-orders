using AutoMapper;
using fastfood_orders.Domain.Entity;

namespace fastfood_orders.Application.UseCases.GetPendingOrders;

public class GetPendingOrdersMapper : Profile
{
    public GetPendingOrdersMapper()
    {
        _ = CreateMap<OrderEntity, GetPendingOrdersOrder>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => MapOrderedItems(src.OrderedItems)));
    }

    private IEnumerable<GetPendingOrdersProductData> MapOrderedItems(IEnumerable<OrderedItemEntity> orderedItems)
    {
        return orderedItems.Select(item => new GetPendingOrdersProductData
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
        });
    }
}
