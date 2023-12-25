namespace ServiceBusProducer.Requests;

public record CreateCustomerRequest(Guid Id, string FullName, DateTime DateOfBirth);