using System.Diagnostics.Contracts;

namespace ComfortablyInject;

public class ServiceProvider : IServiceProvider
{
    private readonly Dictionary<Type, Func<object>> _transientTypes = new();
    private readonly Dictionary<Type, Lazy<object>> _singletonTypes = new();
    
    internal ServiceProvider(IEnumerable<ServiceDescriptor> serviceDescriptors)
    {
        GenerateServiceResolvers(serviceDescriptors);
    }

    private void GenerateServiceResolvers(IEnumerable<ServiceDescriptor> serviceDescriptors)
    {
        foreach (var serviceDescriptor in serviceDescriptors)
        {
            switch (serviceDescriptor.Lifetime)
            {
                case ServiceLifetime.Scoped:
                    break;
                case ServiceLifetime.Singleton:
                    if (serviceDescriptor.ImplementationInstance is not null)
                        _singletonTypes[serviceDescriptor.ServiceType] = new Lazy<object>(serviceDescriptor.ImplementationInstance);
                    else if (serviceDescriptor.ImplementationFactory is not null)
                        _singletonTypes[serviceDescriptor.ServiceType] = new Lazy<object>(() => serviceDescriptor.ImplementationFactory(this));
                    else
                        _singletonTypes[serviceDescriptor.ServiceType] = new Lazy<object>(() => Activator.CreateInstance(
                            serviceDescriptor.ImplementationType, 
                            GetConstructorParameters(serviceDescriptor))!
                        );
                    break;
                case ServiceLifetime.Transient:
                    if (serviceDescriptor.ImplementationFactory is not null)
                        _transientTypes[serviceDescriptor.ServiceType] = () => serviceDescriptor.ImplementationFactory(this);
                    else 
                        _transientTypes[serviceDescriptor.ServiceType] = () => Activator.CreateInstance(
                        serviceDescriptor.ImplementationType,
                        GetConstructorParameters(serviceDescriptor))!;
                    break;
            }
        }

        object?[] GetConstructorParameters(ServiceDescriptor descriptor)
        {
            var constructorInfo = descriptor.ImplementationType.GetConstructors().First();
            var parameters = constructorInfo.GetParameters().Select(x => GetService(x.ParameterType)).ToArray();

            return parameters;
        }
    }

    public TService? GetService<TService>()
    {
        return (TService?)GetService(typeof(TService));
    }
    
    public object? GetService(Type serviceType)
    {
        var service = _singletonTypes.GetValueOrDefault(serviceType);
        if (service is not null)
            return service.Value;

        var transientServiceFactory = _transientTypes.GetValueOrDefault(serviceType);
        return transientServiceFactory?.Invoke();
    }
}