using fastfood_orders.Application.UseCases.UpdateOrder;
using fastfood_orders.Domain.Contracts.RabbitMq;
using fastfood_orders.Domain.Entity;
using fastfood_orders.Infra.RabbitMq.Message;
using fastfood_orders.Infra.RabbitMq.Settings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace fastfood_orders.Infra.RabbitMq;

[ExcludeFromCodeCoverage]
public class RabbitService : IRabbitService, IDisposable
{
    private readonly RabbitMqSettings _settings;
    private IConnection _connection;
    private IModel _channel;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

    public RabbitService(IOptions<RabbitMqSettings> options, IServiceScopeFactory serviceScopeFactory)
    {
        _settings = options.Value;
        _serviceScopeFactory = serviceScopeFactory;
        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        ConnectionFactory factory = new()
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password,
            Port = 5671
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _settings.ReplyQueueName,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

        _channel.QueueDeclare(queue: _settings.OrderQueueName,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

        _channel.QueueDeclare(queue: _settings.PaymentQueueName,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

        EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out TaskCompletionSource<string>? tcs))
            {
                byte[] body = ea.Body.ToArray();
                string response = Encoding.UTF8.GetString(body);
                tcs.SetResult(response);
            }
        };

        _channel.BasicConsume(
            consumer: consumer,
            queue: _settings.ReplyQueueName,
            autoAck: true);
    }

    public void StartConsuming()
    {
        EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            ProcessMessage(message);

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _settings.OrderQueueName,
                             autoAck: false,
                             consumer: consumer);
    }

    private void ProcessMessage(string message)
    {
        var request = JsonSerializer.Deserialize<UpdateOrderRequest>(message);

        if (request == null)
            return;

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var response = mediator.Send(request, default).Result;
        }
    }

    public Task<string> Publish(OrderEntity orderEntity)
    {
        TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
        string correlationId = Guid.NewGuid().ToString();
        _callbackMapper[correlationId] = tcs;

        PaymentMessageRequest message = new PaymentMessageRequest(orderEntity);

        byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        IBasicProperties properties = _channel.CreateBasicProperties();
        properties.DeliveryMode = 2;
        properties.ReplyTo = _settings.ReplyQueueName;
        properties.CorrelationId = correlationId;

        _channel.BasicPublish(exchange: string.Empty,
                             routingKey: _settings.PaymentQueueName,
                             basicProperties: properties,
                             body: body);

        return tcs.Task;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
