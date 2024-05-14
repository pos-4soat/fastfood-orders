using AutoMapper;
using fastfood_orders.Domain.Entity;

namespace fastfood_orders.Application.UseCases.GetOrderById;

public class GetOrderByIdMapper : Profile
{

    public GetOrderByIdMapper()
    {
        _ = CreateMap<OrderEntity, GetOrderByIdResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => MapOrderedItems(src.OrderedItems)));
    }

    private IEnumerable<GetOrderByIdProductData> MapOrderedItems(IEnumerable<OrderedItemEntity> orderedItems)
    {
        return orderedItems.Select(item => new GetOrderByIdProductData
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
        });
    }
}
