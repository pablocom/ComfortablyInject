namespace ComfortablyInject;

public class ServiceDescriptor
{
    public Type ServiceType { get; init; }
    public Type ImplementationType { get; init; }
    public ServiceLifetime Lifetime { get; init; }
    public object? ImplementationInstance { get; init; }
    public Func<ServiceProvider, object>? ImplementationFactory { get; init; }
    
    public static ServiceDescriptor FromSingletonInstance(object singletonInstance)
    {
        var type = singletonInstance.GetType();
        return new ServiceDescriptor
        {
            ServiceType = type,
            ImplementationType = type,
            ImplementationInstance = singletonInstance,
            Lifetime = ServiceLifetime.Singleton
        };
    }   

    public static ServiceDescriptor FromImplementationFactory<TService>(Func<ServiceProvider, TService> implementationFactory, 
        ServiceLifetime lifetime)
        where TService : class
    {
        var type = typeof(TService);
        return new ServiceDescriptor
        {
            ServiceType = type,
            ImplementationType = type,
            ImplementationFactory = implementationFactory,
            Lifetime = lifetime
        };
    }
}