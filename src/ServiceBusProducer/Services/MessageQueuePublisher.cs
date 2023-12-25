using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace ServiceBusProducer.Services;

public class MessageQueuePublisher: IMessagePublisher
{ 
    private readonly IQueueClient _queueClient;

    public MessageQueuePublisher(IQueueClient queueClient)
    {
        _queueClient = queueClient;
    }
    public Task Publish<T>(T obj,string? sessionId = default)
    {
        var objectAsText = JsonConvert.SerializeObject(obj);
        var message = new Message(Encoding.UTF8.GetBytes(objectAsText));
        message.UserProperties["messageType"] = typeof(T).Name;
        return _queueClient.SendAsync(message);
    }
}