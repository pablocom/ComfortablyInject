namespace ComfortablyInject.UnitTests;

public interface IConsoleWriter
{
    void WriteLine(string text);
}

public class ConsoleWriter : IConsoleWriter
{
    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }
}

public interface IIdGenerator
{
    public Guid Guid { get; }
}

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