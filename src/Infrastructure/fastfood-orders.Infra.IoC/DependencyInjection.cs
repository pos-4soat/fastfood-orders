using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Domain.Contracts.Http;
using fastfood_orders.Domain.Contracts.RabbitMq;
using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Infra.Http;
using fastfood_orders.Infra.RabbitMq;
using fastfood_orders.Infra.RabbitMq.Settings;
using fastfood_orders.Infra.SqlServer.Context;
using fastfood_orders.Infra.SqlServer.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace fastfood_orders.Infra.IoC;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureBehavior();
        services.ConfigureSettings(configuration);
        services.ConfigureServices();
        services.ConfigureAutomapper();
        services.ConfigureMediatr();
        services.ConfigureFluentValidation();
        services.ConfigureHttpClient(configuration);
        services.ConfigureDatabase(configuration);
    }

    private static void ConfigureBehavior(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
    }
    private static void ConfigureAutomapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Result).Assembly);
    }

    private static void ConfigureMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Result).Assembly));
    }

    private static void ConfigureFluentValidation(this IServiceCollection service)
    {
        service.AddValidatorsFromAssemblyContaining<Result>();
        service.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        service.AddFluentValidationRulesToSwagger();
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddSingleton<IProducerService, ProducerService>();
    }

    private static void ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        IConfiguration externalConfig = configuration.GetSection("Http");
        string baseUrl = externalConfig.GetSection("Product").Value;

        services.AddTransient<IProductHttpClient>(provider =>
        {
            string? baseAddress = baseUrl;
            return new ProductHttpClient(baseAddress);
        });
    }

    private static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
    }

    private static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"),
                                                     b => b.MigrationsAssembly("fastfood-orders.Infra.SqlServer")).LogTo(s => Console.WriteLine(s)));
    }
}
