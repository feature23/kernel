namespace F23.Kernel;

/// <summary>
/// Provides extension methods for registering handlers in the service collection.
/// </summary>
public static class KernelServiceCollectionExtensions
{
    /// <summary>
    /// Registers a query handler in the service collection.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TQueryHandler">The type of the query handler.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <remarks>
    /// A <see cref="DataAnnotationsValidator{T}"/> is also registered for the query.
    /// </remarks>
    public static void RegisterQueryHandler<TQuery, TResult, TQueryHandler>(this IServiceCollection services)
        where TQuery : IQuery<TResult>
        where TQueryHandler : class, IQueryHandler<TQuery, TResult>
    {
        services.AddTransient<IValidator<TQuery>, DataAnnotationsValidator<TQuery>>();
        services.AddTransient<IQueryHandler<TQuery, TResult>, TQueryHandler>();
    }

    /// <summary>
    /// Registers a command handler in the service collection.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TCommandHandler">The type of the command handler.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <remarks>
    /// A <see cref="DataAnnotationsValidator{T}"/> is also registered for the command.
    /// </remarks>
    public static void RegisterCommandHandler<TCommand, TResult, TCommandHandler>(this IServiceCollection services)
        where TCommand : ICommand<TResult>
        where TCommandHandler : class, ICommandHandler<TCommand, TResult>
    {
        services.AddTransient<IValidator<TCommand>, DataAnnotationsValidator<TCommand>>();
        services.AddTransient<ICommandHandler<TCommand, TResult>, TCommandHandler>();
    }

    /// <summary>
    /// Registers a command handler in the service collection.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <typeparam name="TCommandHandler">The type of the command handler.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <remarks>
    /// A <see cref="DataAnnotationsValidator{T}"/> is also registered for the command.
    /// </remarks>
    public static void RegisterCommandHandler<TCommand, TCommandHandler>(this IServiceCollection services)
        where TCommand : ICommand
        where TCommandHandler : class, ICommandHandler<TCommand>
    {
        services.AddTransient<IValidator<TCommand>, DataAnnotationsValidator<TCommand>>();
        services.AddTransient<ICommandHandler<TCommand>, TCommandHandler>();
    }

    /// <summary>
    /// Registers an event handler in the service collection.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    /// <param name="services">The service collection.</param>
    public static void RegisterEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
        where TEvent : IDomainEvent
        where TEventHandler : class, IEventHandler<TEvent>
    {
        services.AddTransient<IEventHandler<TEvent>, TEventHandler>();
    }
}
