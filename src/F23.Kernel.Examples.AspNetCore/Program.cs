using System.Reflection;
using F23.Kernel;
using F23.Kernel.AspNetCore;
using F23.Kernel.Examples.AspNetCore.Core;
using F23.Kernel.Examples.AspNetCore.Infrastructure;
using F23.Kernel.Examples.AspNetCore.UseCases.GetWeatherForecast;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var version = typeof(Result).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
              ?? "0.0.0";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc($"v{version}", new OpenApiInfo
    {
        Title = "F23.Kernel Examples",
        Version = $"v{version}",
    });
});

builder.Services.AddSingleton<IWeatherForecastRepository, MockWeatherForecastRepository>();
builder.Services.RegisterQueryHandler<GetWeatherForecastQuery, GetWeatherForecastQueryResult, GetWeatherForecastQueryHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/v{version}/swagger.json", $"F23.Kernel Examples v{version}");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/weatherforecast", async (IQueryHandler<GetWeatherForecastQuery, GetWeatherForecastQueryResult> queryHandler) =>
    {
        var result = await queryHandler.Handle(new GetWeatherForecastQuery());

        return result.ToMinimalApiResult();
    })
    .WithName("GetWeatherForecast");

app.MapResultsEndpoints();

app.Run();
