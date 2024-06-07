using AutoMapper;
using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Domain.Contracts.Http;
using fastfood_orders.Domain.Contracts.RabbitMq;
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
        IProductHttpClient httpClient,
        IProducerService producer) : IRequestHandler<CreateOrderRequest, Result<CreateOrderResponse>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IProductHttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        private readonly IProducerService _producer = producer ?? throw new ArgumentNullException(nameof(producer));

        public async Task<Result<CreateOrderResponse>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            OrderEntity orderEntity = _mapper.Map<OrderEntity>(request);

            List<OrderedItemEntity> products = orderEntity.OrderedItems.ToList();

            decimal totalAmount = 0;

            foreach (OrderedItemEntity? item in products)
            {
                ProductData product = await _httpClient.GetProductInfo(item.ProductId, cancellationToken);

                if (product == null)
                    return Result<CreateOrderResponse>.Failure("OBE004");

                totalAmount += product.Price * item.Quantity;
                item.Name = product.Name;
                item.Price = product.Price;
            }

            orderEntity.Amount = totalAmount;

            await _orderRepository.AddOrderAsync(orderEntity, cancellationToken);

            string qrCode = _producer.Publish(orderEntity).Result;

            if (string.IsNullOrEmpty(qrCode))
            {
                orderEntity.Status = OrderStatus.Canceled;
                await _orderRepository.EditOrderAsync(orderEntity, cancellationToken);
                return Result<CreateOrderResponse>.Failure("OBE015");
            }

            CreateOrderResponse response = new()
            {
                Id = orderEntity.Id,
                Status = orderEntity.Status,
                TotalPrice = totalAmount,
                PaymentQRCode = qrCode
            };

            return Result<CreateOrderResponse>.Success(response, StatusResponse.CREATED);
        }
    }
}
