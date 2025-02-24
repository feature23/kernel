using F23.Kernel.Examples.AspNetCore.UseCases.GetWeatherForecast;

namespace F23.Kernel.Examples.AspNetCore.Core;

internal interface IWeatherForecastRepository
{
    Task<IReadOnlyList<WeatherForecast>> GetForecast(int daysIntoTheFuture, CancellationToken cancellationToken = default);
}
