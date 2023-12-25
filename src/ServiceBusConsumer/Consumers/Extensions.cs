using Microsoft.Azure.ServiceBus;

namespace ServiceBusConsumer.Consumers;

public static class Extensions
{
    public static IServiceCollection AddTopicSubscriber(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("ServiceBus");
        var options = section.BindOptions<ServiceBusOptions>();
        
        services.AddHostedService<CustomerSubscriptionService>();

        services.AddSingleton<ISubscriptionClient>(provider =>
        {
            return new SubscriptionClient(options.ConnectionString, options.TopicName, options.SubscriptionName);
        });

        return services;
    }
    
    public static IServiceCollection AddConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("ServiceBus");
        var options = section.BindOptions<ServiceBusOptions>();
        
        services.AddHostedService<CustomerConsumerService>();

        services.AddSingleton<IQueueClient>(provider =>
        {
            return new QueueClient(options.ConnectionString, options.QueueName);
        });

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