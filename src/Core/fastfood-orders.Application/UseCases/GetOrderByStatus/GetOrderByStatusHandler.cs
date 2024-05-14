using AutoMapper;
using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Domain.Contracts.Http;
using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Domain.Models.Response;
using MediatR;

namespace fastfood_orders.Application.UseCases.GetOrderByStatus;

public class GetOrderByStatusHandler : IRequestHandler<GetOrderByStatusRequest, Result<GetOrderByStatusResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IProductHttpClient _httpClient;
    public GetOrderByStatusHandler(
    IOrderRepository orderRepository,
        IMapper mapper,
        IProductHttpClient httpClient)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<Result<GetOrderByStatusResponse>> Handle(GetOrderByStatusRequest request, CancellationToken cancellationToken)
    {
        List<Domain.Entity.OrderEntity> result = (await _orderRepository.GetOrderByStatus(request.Status, cancellationToken)).ToList();

        IEnumerable<Task<GetOrderByStatusOrder>> orders = result.Select(async orderEntity =>
        {
            GetOrderByStatusOrder order = _mapper.Map<GetOrderByStatusOrder>(orderEntity);

            order.Items = await Task.WhenAll(orderEntity.OrderedItems.Select(async item =>
            {
                ProductData product = await _httpClient.GetProductInfo(item.ProductId, cancellationToken);
                return new GetOrderByStatusProductData
                {
                    ProductId = item.ProductId,
                    Price = product.Price,
                    Name = product.Name,
                    Quantity = item.Quantity,
                };
            }));

            return order;
        });

        List<GetOrderByStatusOrder> ordersList = (await Task.WhenAll(orders)).ToList();

        GetOrderByStatusResponse response = new GetOrderByStatusResponse(ordersList);

        return Result<GetOrderByStatusResponse>.Success(response);
    }
}
