using ServiceBusConsumer.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConsumer(builder.Configuration);
    
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();