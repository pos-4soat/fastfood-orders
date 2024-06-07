﻿using System.Diagnostics.CodeAnalysis;

namespace fastfood_orders.Infra.RabbitMq.Settings;


[ExcludeFromCodeCoverage]
public class RabbitMqSettings
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string QueueName { get; set; }
    public string ReplyQueueName { get; set; }
}
