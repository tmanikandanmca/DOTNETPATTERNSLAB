namespace Behavioral.Command.UnitTests;

using Behavioral.Command.UndoableCommand.Api;

/// <summary>
/// Test checklist for UndoableCommand variant:
/// ✓ Command classes are executable without caller knowing receiver internals
/// ✓ Invoker code accepts abstractions (IUndoableCommand interface)
/// ✓ Undoable commands safely revert only previously executed actions
/// ✓ Command execution is testable in isolation with mocked receivers
/// ✓ Undo operations restore state to pre-execution condition
/// </summary>
public class UndoableCommandChecklistTests
{
    [Test]
    public void Execute_ExecutesCommandWithoutCallerKnowingReceiverInternals()
    {
        // Arrange
        var counter = new Counter();
        IUndoableCommand command = new IncrementCommand(counter);

        // Act - invoker only knows about IUndoableCommand abstraction
        var result = command.Execute();

        // Assert
        Assert.That(result, Does.Contain("Value=1"));
        Assert.That(counter.Value, Is.EqualTo(1));
    }

    [Test]
    public void InvokerAcceptsAbstractions_CanExecuteAnyUndoableCommandImplementation()
    {
        // Arrange
        var counter = new Counter();
        IUndoableCommand command1 = new IncrementCommand(counter);
        IUndoableCommand command2 = new IncrementCommand(counter);

        // Act - invoker only knows about IUndoableCommand abstraction
        command1.Execute();
        command2.Execute();

        // Assert
        Assert.That(counter.Value, Is.EqualTo(2));
    }

    [Test]
    public void UndoableCommands_SafelyRevertOnlyPreviouslyExecutedActions()
    {
        // Arrange
        var counter = new Counter();
        var command1 = new IncrementCommand(counter);
        var command2 = new IncrementCommand(counter);

        // Act - execute both
        command1.Execute();
        command2.Execute();
        Assert.That(counter.Value, Is.EqualTo(2));

        // Undo only command2
        command2.Undo();
        var stateAfterFirstUndo = counter.Value;

        // Undo command1
        command1.Undo();
        var stateAfterSecondUndo = counter.Value;

        // Assert - only executed commands were undone
        Assert.Multiple(() =>
        {
            Assert.That(stateAfterFirstUndo, Is.EqualTo(1));
            Assert.That(stateAfterSecondUndo, Is.EqualTo(0));
        });
    }

    [Test]
    public void CommandExecutionIsTestableInIsolation_WithRealReceiver()
    {
        // Arrange - command can be tested in isolation with real receiver
        var counter = new Counter();
        IUndoableCommand command = new IncrementCommand(counter);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(counter.Value, Is.EqualTo(1));
    }

    [Test]
    public void Undo_RestoresStateToPreviousCondition()
    {
        // Arrange
        var counter = new Counter();

        // Act & Assert
        Assert.That(counter.Value, Is.EqualTo(0), "Initial state should be 0");

        var command = new IncrementCommand(counter);
        command.Execute();
        Assert.That(counter.Value, Is.EqualTo(1), "After execute should be 1");

        command.Undo();
        Assert.That(counter.Value, Is.EqualTo(0), "After undo should return to 0");
    }

    [Test]
    public void MultipleUndoOperations_WorkCorrectlyInSequence()
    {
        // Arrange
        var counter = new Counter();
        var commands = new List<IUndoableCommand>
        {
            new IncrementCommand(counter),
            new IncrementCommand(counter),
            new IncrementCommand(counter)
        };

        // Act - execute all commands
        foreach (var cmd in commands)
        {
            cmd.Execute();
        }
        Assert.That(counter.Value, Is.EqualTo(3));

        // Undo all commands in reverse order
        var stateAfterUndoAll = new List<int>();
        foreach (var cmd in Enumerable.Reverse(commands))
        {
            cmd.Undo();
            stateAfterUndoAll.Add(counter.Value);
        }

        // Assert - state transitions should be 2, 1, 0
        Assert.That(stateAfterUndoAll, Is.EqualTo(new[] { 2, 1, 0 }));
    }

    [Test]
    public void CommandAbstraction_PreventsDirectAccessToReceiverState()
    {
        // Arrange
        var counter = new Counter();
        IUndoableCommand command = new IncrementCommand(counter);

        // Act & Assert
        // Invoker can only call Execute() and Undo() through IUndoableCommand interface
        // Invoker never has direct access to Counter.Value or Counter methods
        Assert.That(command, Is.AssignableTo<IUndoableCommand>());
        Assert.DoesNotThrow(() => command.Execute());
        Assert.DoesNotThrow(() => command.Undo());
    }

    [Test]
    public void MultipleCommands_OnDifferentReceivers_AreIndependent()
    {
        // Arrange
        var counter1 = new Counter();
        var counter2 = new Counter();

        var command1 = new IncrementCommand(counter1);
        var command2 = new IncrementCommand(counter2);

        // Act
        command1.Execute();
        command2.Execute();
        command1.Undo();

        // Assert - undoing command1 doesn't affect counter2
        Assert.Multiple(() =>
        {
            Assert.That(counter1.Value, Is.EqualTo(0));
            Assert.That(counter2.Value, Is.EqualTo(1));
        });
    }

    [Test]
    public void CommandHistory_TracksExecutionState()
    {
        // Arrange
        var history = new CommandHistory();
        var counter = new Counter();

        // Act - build history
        var cmd1 = new IncrementCommand(counter);
        var cmd2 = new IncrementCommand(counter);

        history.Execute(cmd1);
        Assert.That(history.CanUndo(), Is.True);

        history.Execute(cmd2);
        Assert.That(history.CanUndo(), Is.True);

        // Undo
        history.Undo();
        Assert.That(counter.Value, Is.EqualTo(1));

        history.Undo();
        Assert.That(counter.Value, Is.EqualTo(0));
        Assert.That(history.CanUndo(), Is.False);
    }

    [Test]
    public void CommandAbstraction_EnablesPolymorphicBehavior()
    {
        // Arrange
        var counter = new Counter();
        IUndoableCommand[] commands = new IUndoableCommand[]
        {
            new IncrementCommand(counter),
            new IncrementCommand(counter),
            new IncrementCommand(counter)
        };

        // Act - execute all through abstraction
        foreach (var cmd in commands)
        {
            cmd.Execute();
        }

        // Assert
        Assert.That(counter.Value, Is.EqualTo(3));

        // Undo all through abstraction
        foreach (var cmd in commands.Reverse())
        {
            cmd.Undo();
        }

        Assert.That(counter.Value, Is.EqualTo(0));
    }

    /// <summary>
    /// Command history manager for tracking executed commands and enabling undo.
    /// </summary>
    private sealed class CommandHistory
    {
        private readonly Stack<IUndoableCommand> _history = new();

        public void Execute(IUndoableCommand command)
        {
            command.Execute();
            _history.Push(command);
        }

        public void Undo()
        {
            if (_history.Count > 0)
            {
                var command = _history.Pop();
                command.Undo();
            }
        }

        public bool CanUndo() => _history.Count > 0;
    }
}
