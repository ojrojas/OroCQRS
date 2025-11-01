using CreateWebUserCommand;
using OroCQRS.Core.Extensions;
using OroCQRS.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Use Core's extension to register services
builder.Services.AddCqrsHandlers();

var app = builder.Build();

// Define endpoints
app.MapPost("/users", async (CreateUserCommand command, ISender handler, CancellationToken cancellationToken) =>
{
    await handler.Send(command, cancellationToken);
    return Results.Ok("User created successfully");
});

app.Run();