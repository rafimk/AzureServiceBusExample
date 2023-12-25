namespace ServiceBusProducer.Services;

public interface IMessagePublisher
{
    Task Publish<T>(T obj, string? sessionId = default);
}