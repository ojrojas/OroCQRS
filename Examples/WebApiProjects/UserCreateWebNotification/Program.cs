using UserCreateWebNotification;
using OroCQRS.Core.Extensions;
using OroCQRS.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

    // Use Core's extension to register services
builder.Services.AddCqrsHandlers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Define endpoints
app.MapPost("/notifications", async (
    UserCreatedNotification notification, 
    ISender handler) =>
{
    await handler.Send(notification, CancellationToken.None);
    return Results.Ok("Notification processed successfully");
});

app.Run();