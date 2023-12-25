using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace ServiceBusProducer.Services;

public class MessageTopicPublisher : IMessagePublisher
{
    private readonly ITopicClient _topicClient;

    public MessageTopicPublisher(ITopicClient topicClient)
    {
        _topicClient = topicClient;
    }
    public Task Publish<T>(T obj,string? sessionId = default)
    {
        var objectAsText = JsonConvert.SerializeObject(obj);
        var message = new Message(Encoding.UTF8.GetBytes(objectAsText));
        message.UserProperties["messageType"] = typeof(T).Name;
        return _topicClient.SendAsync(message);
    }
}