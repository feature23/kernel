namespace F23.Kernel.Examples.AspNetCore.UseCases.GetWeatherForecast;

class GetWeatherForecastQueryResult
{
    public required IReadOnlyList<WeatherForecast> Forecast { get; init; }
}

class GetWeatherForecastQuery : IQuery<GetWeatherForecastQueryResult>
{
    public int DaysIntoTheFuture { get; init; } = 5;
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
