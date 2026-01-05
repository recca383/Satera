using Microsoft.AspNetCore.Mvc;
using Satera_Api;
using Satera_Api.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGetMLAnalysisHandler, GetMLAnalysisHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("getMl/", async (
                [FromBody]Request request,
                IGetMLAnalysisHandler handler,
                CancellationToken cancellationToken) =>
{
    var command = new GetMLAnalysisCommand(request.name);

    var result = await handler.Handle(command, cancellationToken);

    return result.Match(Results.Ok, CustomResults.Problem);
});

app.Run();

