using AutoMapper;
using fastfood_orders.Domain.Entity;

namespace fastfood_orders.Application.UseCases.GetOrderByStatus;

public class GetOrderByStatusMapper : Profile
{
    public GetOrderByStatusMapper()
    {
        _ = CreateMap<OrderEntity, GetOrderByStatusOrder>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => MapOrderedItems(src.OrderedItems)));
    }

    private IEnumerable<GetOrderByStatusProductData> MapOrderedItems(IEnumerable<OrderedItemEntity> orderedItems)
    {
        return orderedItems.Select(item => new GetOrderByStatusProductData
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
        });
    }
}
