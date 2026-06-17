namespace Behavioral.Command.CompositeCommand.Api;

public interface ICommand
{
    string Execute();
}

public sealed class WriteCommand(List<string> log, string message) : ICommand
{
    public string Execute()
    {
        log.Add(message);
        return message;
    }
}

public sealed class CompositeCommand(params ICommand[] commands) : ICommand
{
    public string Execute() => string.Join(" | ", commands.Select(command => command.Execute()));
}

public static class CompositeCommandDemo
{
    public static object Create()
    {
        var log = new List<string>();
        ICommand composite = new CompositeCommand(
            new WriteCommand(log, "Validate"),
            new WriteCommand(log, "Persist"),
            new WriteCommand(log, "Publish"));

        var result = composite.Execute();

        return new
        {
            Pattern = "Command",
            Variant = "Composite Command",
            Result = result,
            Log = log
        };
    }
}
