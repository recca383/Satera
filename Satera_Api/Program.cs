using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Satera_Api;
using Satera_Api.Application;
using Satera_Api.Data;
using Satera_Api.Helper;
using Satera_Api.ML;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGetMLAnalysisHandler, GetMLAnalysisHandler>();
builder.Services.AddScoped<IDataPreparationHandler, DataPreparationHandler>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("App_Categorization")));

builder.Services.AddScoped<IAppDbContext, AppDbContext>();

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
                IAppDbContext dbContext,
                CancellationToken cancellationToken) =>
{
    var command = new GetMLAnalysisCommand(
        request.Gwa,
        request.TrackingDurationDays,
        request.TotalScreenTime,
        request.TotalAppsTracked,
        request.Pickups,
        request.DeviceUnlocks,
        request.Apps,
        request.CollectionTimestamp,
        request.Platform
        );
    
    
    var result = await handler.Handle(command, cancellationToken);

    return result.Match(Results.Ok, CustomResults.Problem);
});

app.Run();

