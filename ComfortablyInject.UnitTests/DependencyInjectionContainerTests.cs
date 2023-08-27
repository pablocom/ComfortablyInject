namespace ComfortablyInject.UnitTests;

public class DependencyInjectionContainerTests
{
    [Fact]
    public void ResolvesSingletonServiceAlwaysToTheSameInstance()
    {
        var containerBuilder = new ServiceCollection();

        containerBuilder.AddSingleton<IConsoleWriter, ConsoleWriter>();

        var serviceProvider = containerBuilder.BuildServiceProvider();

        var consoleWriter = serviceProvider.GetService<IConsoleWriter>();
        consoleWriter.Should().NotBeNull();
        consoleWriter.Should().BeOfType<ConsoleWriter>();

        var consoleWriterSecondTimeResolved = serviceProvider.GetService<IConsoleWriter>();
        consoleWriter.Should()
            .Be(consoleWriterSecondTimeResolved);
    }
    
    [Fact]
    public void ResolvesTransientServiceAlwaysCreatingNewInstances()
    {
        var containerBuilder = new ServiceCollection();

        containerBuilder.AddTransient<IConsoleWriter, ConsoleWriter>();

        var serviceProvider = containerBuilder.BuildServiceProvider();

        var consoleWriter = serviceProvider.GetService<IConsoleWriter>();
        consoleWriter.Should().NotBeNull();
        consoleWriter.Should().BeOfType<ConsoleWriter>();

        var consoleWriterSecondTimeResolved = serviceProvider.GetService<IConsoleWriter>();
        consoleWriter.Should().NotBe(consoleWriterSecondTimeResolved);
    }

    [Fact]
    public void MultipleServicesResolutionTest()
    {
        var containerBuilder = new ServiceCollection();

        containerBuilder.AddSingleton<IConsoleWriter, ConsoleWriter>();
        containerBuilder.AddTransient<IIdGenerator, IdGenerator>();
        
        var serviceProvider = containerBuilder.BuildServiceProvider();
        var idGenerator = serviceProvider.GetService<IIdGenerator>() as IdGenerator;
        idGenerator!.PrintId();
    }
}