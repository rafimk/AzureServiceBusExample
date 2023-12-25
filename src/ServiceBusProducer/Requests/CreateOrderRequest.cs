namespace ServiceBusProducer.Requests;

public record CreateOrderRequest(Guid Id, string ProductName);