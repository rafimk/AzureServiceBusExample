using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServiceBusContracts;

namespace ServiceBusConsumer.Consumers;

public class CustomerConsumerService : BackgroundService
{
    private readonly IQueueClient _queueClient;
    private readonly ILogger<CustomerConsumerService> _logger;

    public CustomerConsumerService(IQueueClient queueClient, ILogger<CustomerConsumerService> logger)
    {
        _queueClient = queueClient;
        _logger = logger;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _queueClient.RegisterMessageHandler((message, stoppingToken) =>
        {
            var customerCreated = JsonConvert.DeserializeObject<CustomerCreated>(Encoding.UTF8.GetString(message.Body));
            
            Console.WriteLine($"New customer with {customerCreated.FullName} and id {customerCreated.Id}");

            return _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }, new MessageHandlerOptions(args => Task.CompletedTask)
        {
            AutoComplete = false,
            MaxConcurrentCalls = 1
        });
        return Task.CompletedTask;
    }
}