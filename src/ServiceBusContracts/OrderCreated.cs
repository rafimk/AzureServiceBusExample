namespace ServiceBusContracts;

public record OrderCreated
{
    public Guid Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
}