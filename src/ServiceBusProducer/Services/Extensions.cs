using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Azure;

namespace ServiceBusProducer.Services;

public static class Extensions
{
    public static IServiceCollection AddAzureServiceBusTopic(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("ServiceBus");
        var options = section.BindOptions<ServiceBusOptions>();

        services.AddSingleton<ITopicClient>(provider =>
        {
            return new TopicClient(options.ConnectionString, options.TopicName);
        });

        services.AddSingleton<IMessagePublisher, MessageTopicPublisher>();

        return services;
    }
    
    public static IServiceCollection AddAzureServiceBusQueue(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("ServiceBus");
        var options = section.BindOptions<ServiceBusOptions>();

        services.AddSingleton<IQueueClient>(provider => 
        {
            return new QueueClient(options.ConnectionString, options.QueueName);
        });

        services.AddSingleton<IMessagePublisher, MessageQueuePublisher>();

        return services;
    }
    
    public static IServiceCollection AddAzureClient(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("ServiceBus");
        var options = section.BindOptions<ServiceBusOptions>();
        services.Configure<ServiceBusOptions>(section);
        services.AddAzureClients(builder =>
        {
            builder.AddServiceBusClient(options.ConnectionString);
        });
        
        services.AddSingleton<IMessagePublisher, MessageQueueWithSessionPublisher>();
        return services;
    }
    
    public static T BindOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        => BindOptions<T>(configuration.GetSection(sectionName));

    public static T BindOptions<T>(this IConfigurationSection section) where T : new()
    {
        var options = new T();
        section.Bind(options);
        return options;
    }
}