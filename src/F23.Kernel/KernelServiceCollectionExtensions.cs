namespace F23.Kernel;

public static class KernelServiceCollectionExtensions
{
    public static void RegisterQueryHandler<TQuery, TResult, TQueryHandler>(this IServiceCollection services)
        where TQuery : IQuery<TResult>
        where TQueryHandler : class, IQueryHandler<TQuery, TResult>
    {
        services.AddTransient<IValidator<TQuery>, DataAnnotationsValidator<TQuery>>();
        services.AddTransient<IQueryHandler<TQuery, TResult>, TQueryHandler>();
    }

    public static void RegisterCommandHandler<TCommand, TResult, TCommandHandler>(this IServiceCollection services)
        where TCommand : ICommand<TResult>
        where TCommandHandler : class, ICommandHandler<TCommand, TResult>
    {
        services.AddTransient<IValidator<TCommand>, DataAnnotationsValidator<TCommand>>();
        services.AddTransient<ICommandHandler<TCommand, TResult>, TCommandHandler>();
    }

    public static void RegisterCommandHandler<TCommand, TCommandHandler>(this IServiceCollection services)
        where TCommand : ICommand
        where TCommandHandler : class, ICommandHandler<TCommand>
    {
        services.AddTransient<IValidator<TCommand>, DataAnnotationsValidator<TCommand>>();
        services.AddTransient<ICommandHandler<TCommand>, TCommandHandler>();
    }

    public static void RegisterEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
        where TEvent : IDomainEvent
        where TEventHandler : class, IEventHandler<TEvent>
    {
        services.AddTransient<IEventHandler<TEvent>, TEventHandler>();
    }
}
