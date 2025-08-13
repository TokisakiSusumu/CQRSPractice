using Application.Abstractions.Messaging;
using Application.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;
public static class DependencyInjection
{
    public static IServiceCollection AddMediatoR(this IServiceCollection services) 
    {
        services.AddScoped<ISender, Sender>();
        Assembly? assembly = Assembly.GetExecutingAssembly();
        //get concrete handler types to be registered as the one that will actaully being executed
        IEnumerable<Type>? handlerTypes = assembly.GetTypes()
           .Where(type => type.IsClass && !type.IsAbstract && !type.IsGenericType)
           .Where(type => type.GetInterfaces().Any(@interface => @interface.IsGenericType &&
               (@interface.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                @interface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                @interface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))));

        //register all the handlers, but coupling the interface with the concrete type
        foreach (var handlerType in handlerTypes)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(@interface => @interface.IsGenericType &&
                    (@interface.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                     @interface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                     @interface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));
            foreach (var @interface in interfaces)
            {
                //Web layer only need to inject the interface in order to send any query to use concrete implementation
                services.AddScoped(@interface, handlerType);
            }
        }
        return services;
    }
}
