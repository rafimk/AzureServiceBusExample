using Microsoft.AspNetCore.Mvc;
using ServiceBusContracts;
using ServiceBusProducer.Requests;
using ServiceBusProducer.Services;

namespace ServiceBusProducer.Controllers;

[ApiController]
[Route("[controller]")]
public class SessionMessagingController : ControllerBase
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<SessionMessagingController> _logger;

    public SessionMessagingController(IMessagePublisher messagePublisher, ILogger<SessionMessagingController> logger)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }
    
    [HttpPost("session/customer")]
    public async Task<ActionResult> PublishCustomer(CreateCustomerRequest request)
    {
        var customerCreated = new CustomerCreated
        {
            Id = request.Id,
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth
        };

        await _messagePublisher.Publish(customerCreated, "test");
        return Ok();
    }
}