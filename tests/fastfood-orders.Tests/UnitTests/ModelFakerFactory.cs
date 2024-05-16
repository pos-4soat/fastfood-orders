using AutoFixture;
using Bogus;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Domain.Enum;

namespace fastfood_orders.Tests.UnitTests;

public class ModelFakerFactory(Fixture autoFixture, Faker faker)
{
    private readonly Fixture _autoFixture = autoFixture;
    private readonly Faker _faker = faker;

    public TRequest GenerateRequest<TRequest>()
        => _autoFixture.Build<TRequest>()
            .Create();

    public OrderEntity GenerateOrderEntity()
        => _autoFixture.Build<OrderEntity>()
            .With(c => c.OrderedItems, GenerateOrderedItemEntity())
            .Without(c => c.OrderedItems)
            .Create();

    public OrderEntity GenerateOrderEntityStatus(OrderStatus status)
    => _autoFixture.Build<OrderEntity>()
        .With(c => c.OrderedItems, GenerateOrderedItemEntity())
        .With(c => c.Status, status - 1)
        .Without(c => c.OrderedItems)
        .Create();

    public IEnumerable<OrderedItemEntity> GenerateOrderedItemEntity()
    {
        List<OrderedItemEntity> list = [];
        for (int i = 0; i < 3; i++)
        {
            list.Add(_autoFixture.Build<OrderedItemEntity>()
            .With(c => c.Order, default(OrderEntity))
            .Without(c => c.Order)
            .Create());
        }
        return list;
    }
    //=> _autoFixture.Build<OrderedItemEntity>()
    //    .Without(x => x.OrderId)
    //    .Create();

    public IEnumerable<TRequest> GenerateManyRequest<TRequest>()
        => _autoFixture.CreateMany<TRequest>();
}
