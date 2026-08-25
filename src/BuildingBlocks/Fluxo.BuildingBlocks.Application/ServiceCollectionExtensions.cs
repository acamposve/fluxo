using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxo.BuildingBlocks.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();
        return services;
    }

    /// <summary>
    /// Registers every ICommandHandler/IQueryHandler implementation found in the given module
    /// assembly. Called once per module from Fluxo.Api's composition root (Artículo IV: the Host
    /// is the only place allowed to know about every module at once).
    /// </summary>
    public static IServiceCollection AddHandlersFrom(this IServiceCollection services, Assembly assembly)
    {
        var handlerInterfaces = new[] { typeof(ICommandHandler<>), typeof(ICommandHandler<,>), typeof(IQueryHandler<,>) };

        var registrations =
            from type in assembly.GetTypes()
            where type is { IsClass: true, IsAbstract: false }
            from @interface in type.GetInterfaces()
            where @interface.IsGenericType && handlerInterfaces.Contains(@interface.GetGenericTypeDefinition())
            select (Service: @interface, Implementation: type);

        foreach (var (service, implementation) in registrations)
            services.AddScoped(service, implementation);

        return services;
    }
}
