namespace Behavioral.Command.SimpleCommand.Api;

public interface ICommand
{
    string Name { get; }
    string Execute();
    string Undo();
}

public sealed class TextBuffer
{
    public string Content { get; private set; } = string.Empty;

    public void Append(string text) => Content += text;

    public void Replace(string text) => Content = text;
}

public sealed class AppendTextCommand(TextBuffer buffer, string text) : ICommand
{
    public string Name => "AppendText";

    public string Execute()
    {
        buffer.Append(text);
        return buffer.Content;
    }

    public string Undo()
    {
        buffer.Replace(string.Empty);
        return buffer.Content;
    }
}

public sealed class MacroCommand(params ICommand[] commands) : ICommand
{
    public string Name => "MacroCommand";

    public string Execute() => string.Join(" | ", commands.Select(command => command.Execute()));

    public string Undo() => string.Join(" | ", commands.Reverse().Select(command => command.Undo()));
}

public static class CommandDemo
{
    public static object Create()
    {
        var buffer = new TextBuffer();
        var appendHello = new AppendTextCommand(buffer, "Hello");
        var appendWorld = new AppendTextCommand(buffer, " World");
        var macro = new MacroCommand(appendHello, appendWorld);

        var executed = macro.Execute();
        var undone = macro.Undo();

        return new
        {
            Pattern = "Command",
            Executed = executed,
            Undone = undone,
            FinalBuffer = buffer.Content
        };
    }
}
