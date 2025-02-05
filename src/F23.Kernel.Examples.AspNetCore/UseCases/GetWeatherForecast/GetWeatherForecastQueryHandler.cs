using F23.Kernel.Examples.AspNetCore.Core;
using F23.Kernel.Results;

namespace F23.Kernel.Examples.AspNetCore.UseCases.GetWeatherForecast;

internal class GetWeatherForecastQueryHandler(IValidator<GetWeatherForecastQuery> validator, IWeatherForecastRepository repository)
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
