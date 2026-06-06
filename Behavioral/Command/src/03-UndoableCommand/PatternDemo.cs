namespace Behavioral.Command.UndoableCommand.Api;

public interface IUndoableCommand
{
    string Execute();
    string Undo();
}

public sealed class Counter
{
    public int Value { get; private set; }

    public void Increment() => Value++;
    public void Decrement() => Value--;
}

public sealed class IncrementCommand(Counter counter) : IUndoableCommand
{
    public string Execute()
    {
        counter.Increment();
        return $"Value={counter.Value}";
    }

    public string Undo()
    {
        counter.Decrement();
        return $"Value={counter.Value}";
    }
}

public static class UndoableCommandDemo
{
    public static object Create()
    {
        var counter = new Counter();
        var command = new IncrementCommand(counter);

        var afterExecute = command.Execute();
        var afterUndo = command.Undo();

        return new
        {
            Pattern = "Command",
            Variant = "Undoable Command",
            AfterExecute = afterExecute,
            AfterUndo = afterUndo
        };
    }
}
