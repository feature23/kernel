# feature[23] Shared Kernel

[![build](https://github.com/feature23/kernel/actions/workflows/ci_build.yml/badge.svg)](https://github.com/feature23/kernel/actions/workflows/ci_build.yml)

A library of reusable types for implementing Clean Architecture in .NET and ASP.NET Core applications, based heavily on the work of [Steve Smith](https://github.com/ardalis) in the following open-sourcee projects:
- [ASP.NET Core Template](https://github.com/ardalis/CleanArchitecture)
- [Shared Kernel](https://github.com/ardalis/Ardalis.SharedKernel)

For the core functionality, only the `F23.Kernel` library is needed. This library provides types for events, results, query and command handlers, validation, and messaging. For smoother integration with ASP.NET Core, the `F23.Kernel.AspNetCore` library can be used for easily mapping between core result types and ASP.NET Core `IActionResult` and model state.

> **WARNING:** This library is currently in a pre-release state, and breaking changes may occur before reaching version 1.0.

## NuGet Installation
### Core Package
```powershell
dotnet add package F23.Kernel
```

### ASP.NET Core Helper Package
```powershell
dotnet add package F23.Kernel.AspNetCore
```

## Examples

### Query Handler
```csharp
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

class GetWeatherForecastQueryHandler(IValidator<GetWeatherForecastQuery> validator, IWeatherForecastRepository repository)
    : IQueryHandler<GetWeatherForecastQuery, GetWeatherForecastQueryResult>
{
    public async Task<Result<GetWeatherForecastQueryResult>> Handle(GetWeatherForecastQuery query, CancellationToken cancellationToken = default)
    {
        if (await validator.Validate(query, cancellationToken) is ValidationFailedResult failed)
        {
            return Result<GetWeatherForecastQueryResult>.ValidationFailed(failed.Errors);
        }

        var forecast = await repository.GetForecast(query.DaysIntoTheFuture, cancellationToken);

        var result = new GetWeatherForecastQueryResult
        {
            Forecast = forecast
        };

        return Result<GetWeatherForecastQueryResult>.Success(result);
    }
}
```

#### Program.cs
```csharp
builder.Services.RegisterQueryHandler<GetWeatherForecastQuery, GetWeatherForecastQueryResult, GetWeatherForecastQueryHandler>();

// Other code omitted for brevity

app.MapGet("/weatherforecast", async (IQueryHandler<GetWeatherForecastQuery, GetWeatherForecastQueryResult> queryHandler) =>
    {
        var result = await queryHandler.Handle(new GetWeatherForecastQuery());

        return result.ToMinimalApiResult();
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();
```

### Command Handler
> TODO

### Event Sourcing
> TODO
