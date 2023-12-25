using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ServiceBusProducer.Services;

public class MessageQueueWithSessionPublisher : IMessagePublisher
{
    private readonly ServiceBusClient _busClient;
    private readonly ILogger<MessageQueueWithSessionPublisher> _logger;
    private readonly ServiceBusOptions _serviceBusOptions;
    
    
    public MessageQueueWithSessionPublisher(ServiceBusClient busClient, 
        IOptions<ServiceBusOptions> serviceBusOptions,
        ILogger<MessageQueueWithSessionPublisher> logger)
    {
        _serviceBusOptions = serviceBusOptions.Value;
        _busClient = busClient;
        _logger = logger;
    }
    public async Task Publish<T>(T obj, string? sessionId = default)
    {
        var objectAsText = JsonConvert.SerializeObject(obj);
        var messageSessionId = sessionId ?? "mySessionId" ;
       
        ServiceBusSender sender = _busClient.CreateSender(_serviceBusOptions.QueueName);
        ServiceBusMessage message = new ServiceBusMessage(Encoding.UTF8.GetBytes(objectAsText))
        {
            SessionId = messageSessionId,
        };
        
        await sender.SendMessageAsync(message);
    }
}