namespace ComfortablyInject.SampleConsoleApp;

public class IdGenerator : IIdGenerator
{
    private readonly IConsoleWriter _consoleWriter;
    public Guid Guid { get; } = Guid.NewGuid();

    public IdGenerator(IConsoleWriter consoleWriter)
    {
        _consoleWriter = consoleWriter;
    }

    public void PrintId()
    {
        _consoleWriter.WriteLine(Guid.ToString());
    }
}