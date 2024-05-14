using AutoMapper;
using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Domain.Contracts.Http;
using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Enum;
using fastfood_orders.Domain.Models.Response;
using MediatR;

namespace fastfood_orders.Application.UseCases.CreateOrder
{
    public class CreateOrderHandler(
        IMapper mapper,
        IOrderRepository orderRepository,
        IProductHttpClient httpClient) : IRequestHandler<CreateOrderRequest, Result<CreateOrderResponse>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IProductHttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        public async Task<Result<CreateOrderResponse>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            OrderEntity orderEntity = _mapper.Map<OrderEntity>(request);

            IEnumerable<(int Id, int Quantity)> products = orderEntity.OrderedItems.Select(x => (Id: x.ProductId, Quantity: x.Quantity));

            decimal totalAmount = 0;

            foreach ((int id, int quantity) in products)
            {
                ProductData product = await _httpClient.GetProductInfo(id, cancellationToken);

                if (product == null)
                    return Result<CreateOrderResponse>.Failure("OBE004");

                totalAmount += product.Price * quantity;
            }

            orderEntity.Amount = totalAmount;

            await _orderRepository.AddOrderAsync(orderEntity, cancellationToken);

            CreateOrderResponse response = new CreateOrderResponse()
            {
                Id = orderEntity.Id,
                Status = orderEntity.Status,
                TotalPrice = totalAmount
            };

            return Result<CreateOrderResponse>.Success(response, StatusResponse.CREATED);
        }
    }
}
