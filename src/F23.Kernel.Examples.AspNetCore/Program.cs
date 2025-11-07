using F23.Kernel;
using F23.Kernel.AspNetCore;
using F23.Kernel.Examples.AspNetCore.Core;
using F23.Kernel.Examples.AspNetCore.Infrastructure;
using F23.Kernel.Examples.AspNetCore.UseCases.GetWeatherForecast;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IWeatherForecastRepository, MockWeatherForecastRepository>();
builder.Services.RegisterQueryHandler<GetWeatherForecastQuery, GetWeatherForecastQueryResult, GetWeatherForecastQueryHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/weatherforecast", async (IQueryHandler<GetWeatherForecastQuery, GetWeatherForecastQueryResult> queryHandler) =>
    {
        var result = await queryHandler.Handle(new GetWeatherForecastQuery());

        return result.ToMinimalApiResult();
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapResultsEndpoints();

app.Run();
