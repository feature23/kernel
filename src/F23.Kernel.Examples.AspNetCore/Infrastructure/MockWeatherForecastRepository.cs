using F23.Kernel.Examples.AspNetCore.Core;
using F23.Kernel.Examples.AspNetCore.UseCases.GetWeatherForecast;

namespace F23.Kernel.Examples.AspNetCore.Infrastructure;

internal class MockWeatherForecastRepository : IWeatherForecastRepository
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public Task<IReadOnlyList<WeatherForecast>> GetForecast(int daysIntoTheFuture, CancellationToken cancellationToken = default)
    {
        var forecast = Enumerable.Range(1, daysIntoTheFuture).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]
                ))
            .ToArray();

        return Task.FromResult<IReadOnlyList<WeatherForecast>>(forecast);
    }
}
