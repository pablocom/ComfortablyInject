namespace ComfortablyInject;

public sealed class ServiceCollection
{
    private readonly List<ServiceDescriptor> _serviceDescriptors = new();
    
    public ServiceCollection Add(ServiceDescriptor serviceDescriptor)
    {
        _serviceDescriptors.Add(serviceDescriptor);
        return this;
    }

    public ServiceCollection AddSingleton(object implementation)
    {
        _serviceDescriptors.Add(ServiceDescriptor.FromSingletonInstance(implementation));
        return this;
    }
    
    public ServiceCollection AddSingleton<TService>()
        where TService : class
    {
        AddServiceDescriptor(typeof(TService), typeof(TService), ServiceLifetime.Singleton);
        return this;
    }

    public ServiceCollection AddSingleton<TService>(Func<ServiceProvider, TService> implementationFactory)
        where TService : class
    {
        _serviceDescriptors.Add(ServiceDescriptor.FromImplementationFactory(implementationFactory, 
            ServiceLifetime.Singleton));
        return this;
    }
    
    public ServiceCollection AddTransient<TService>(Func<ServiceProvider, TService> implementationFactory)
        where TService : class
    {
        _serviceDescriptors.Add(ServiceDescriptor.FromImplementationFactory(implementationFactory, 
            ServiceLifetime.Transient));
        return this;
    }
    
    public ServiceCollection AddTransient<TService>()
        where TService : class
    {
        AddServiceDescriptor(typeof(TService), typeof(TService), ServiceLifetime.Transient);
        return this;
    }
    
    public ServiceCollection AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService 
    {
        AddServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
        return this;
    }
    
    public ServiceCollection AddTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService 
    {
        AddServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
        return this;
    }

    private void AddServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        var serviceDescriptor = new ServiceDescriptor
        {
            ServiceType = serviceType,
            ImplementationType = implementationType,
            Lifetime = lifetime
        };
        _serviceDescriptors.Add(serviceDescriptor);
    }
    
    public ServiceProvider BuildServiceProvider()
    {
        return new ServiceProvider(_serviceDescriptors);
    }
}