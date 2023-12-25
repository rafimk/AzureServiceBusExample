using Microsoft.AspNetCore.Mvc;
using ServiceBusContracts;
using ServiceBusProducer.Requests;
using ServiceBusProducer.Services;

namespace ServiceBusProducer.Controllers;

[ApiController]
[Route("[controller]")]
public class MessagingController : ControllerBase
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<MessagingController> _logger;

    public MessagingController(IMessagePublisher messagePublisher, ILogger<MessagingController> logger)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    [HttpPost("publish/customer")]
    public async Task<ActionResult> PublishCustomer(CreateCustomerRequest request)
    {
        var customerCreated = new CustomerCreated
        {
            Id = request.Id,
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth
        };

        await _messagePublisher.Publish(customerCreated);
        return Ok();
    }
    
    [HttpPost("publish/order")]
    public async Task<ActionResult> PublishCustomer(CreateOrderRequest request)
    {
        var orderCreated = new OrderCreated
        {
            Id = request.Id,
            ProductName = request.ProductName
        };

        await _messagePublisher.Publish(orderCreated);
        return Ok();
    }
    
    
}