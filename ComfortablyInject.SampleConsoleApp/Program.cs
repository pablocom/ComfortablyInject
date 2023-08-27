using ComfortablyInject;
using ComfortablyInject.SampleConsoleApp;

var services = new ServiceCollection();

services.AddSingleton(provider => new IdGenerator(provider.GetService<ConsoleWriter>()!));

services.AddTransient<IIdGenerator, IdGenerator>();
services.AddSingleton<IConsoleWriter, ConsoleWriter>();
services.AddSingleton(new ConsoleWriter());

var serviceProvider = services.BuildServiceProvider();

var generator1 = (IdGenerator) serviceProvider.GetService<IIdGenerator>()!;
generator1.PrintId();
var generator2 = (IdGenerator) serviceProvider.GetService<IIdGenerator>()!;
generator2.PrintId();

var console = serviceProvider.GetService<ConsoleWriter>()!;
console.WriteLine("Hello from singleton instance");

var idGeneratorImplementation = serviceProvider.GetService<IdGenerator>()!;
console.WriteLine($"{idGeneratorImplementation.Guid}");




