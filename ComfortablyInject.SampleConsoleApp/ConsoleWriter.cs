namespace ComfortablyInject.SampleConsoleApp;

public class ConsoleWriter : IConsoleWriter
{
    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }
}