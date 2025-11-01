using GetWebUserQuery;
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
app.MapGet("/users/{id:guid}", async (Guid id, ISender handler) =>
{
    var result = await handler.Send(new GetUserQuery(id), CancellationToken.None);
    return Results.Ok(result);
});

app.Run();
