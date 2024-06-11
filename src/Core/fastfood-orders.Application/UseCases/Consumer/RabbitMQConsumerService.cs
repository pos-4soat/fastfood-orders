using fastfood_orders.Domain.Contracts.RabbitMq;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_orders.Application.UseCases.Consumer;

[ExcludeFromCodeCoverage]
public class RabbitMQConsumerService(IRabbitService rabbitMQConsumer) : BackgroundService
{
    private readonly IRabbitService _rabbitMQConsumer = rabbitMQConsumer;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            _rabbitMQConsumer.StartConsuming();
            while (!stoppingToken.IsCancellationRequested)
            {
                Task.Delay(100, stoppingToken);
            }
        }, stoppingToken);
    }

    public override void Dispose()
    {
        _rabbitMQConsumer.Dispose();
        base.Dispose();
    }
}
