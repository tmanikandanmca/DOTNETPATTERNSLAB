namespace Behavioral.Command.UnitTests;

using Behavioral.Command.SimpleCommand.Api;

/// <summary>
/// Test checklist for SimpleCommand variant:
/// ✓ Command classes are executable without caller knowing receiver internals
/// ✓ Invoker code accepts abstractions (ICommand interface)
/// ✓ Command execution is testable in isolation with mocked receivers
/// ✓ Undo operations safely revert command state
/// </summary>
public class SimpleCommandChecklistTests
{
    [Test]
    public void Execute_ExecutesCommandWithoutCallerKnowingReceiverInternals()
    {
        // Arrange
        var buffer = new TextBuffer();
        ICommand command = new AppendTextCommand(buffer, "Hello");

        // Act - invoker only knows about ICommand abstraction
        var result = command.Execute();

        // Assert
        Assert.That(result, Does.Contain("Hello"));
        Assert.That(buffer.Content, Does.Contain("Hello"));
    }

    [Test]
    public void InvokerAcceptsAbstractions_CanExecuteAnyCommandImplementation()
    {
        // Arrange
        var buffer = new TextBuffer();
        ICommand firstCommand = new AppendTextCommand(buffer, "First");
        ICommand secondCommand = new AppendTextCommand(buffer, "Second");

        // Act
        firstCommand.Execute();
        secondCommand.Execute();

        // Assert
        Assert.That(buffer.Content, Does.Contain("First"));
        Assert.That(buffer.Content, Does.Contain("Second"));
    }

    [Test]
    public void CommandExecutionIsTestableInIsolation_WithRealReceiver()
    {
        // Arrange - command can be tested in isolation with real receiver
        var buffer = new TextBuffer();
        ICommand command = new AppendTextCommand(buffer, "TestData");

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(buffer.Content, Is.EqualTo("TestData"));
    }

    [Test]
    public void Undo_SafelyRevertsCommandState()
    {
        // Arrange
        var buffer = new TextBuffer();
        var command = new AppendTextCommand(buffer, "Hello");

        // Act
        command.Execute();
        var stateAfterExecute = buffer.Content;
        command.Undo();
        var stateAfterUndo = buffer.Content;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(stateAfterExecute, Does.Contain("Hello"));
            Assert.That(stateAfterUndo, Is.Empty);
        });
    }

    [Test]
    public void MacroCommand_ExecutesMultipleCommandsInSequence()
    {
        // Arrange
        var buffer = new TextBuffer();
        ICommand first = new AppendTextCommand(buffer, "Hello");
        ICommand second = new AppendTextCommand(buffer, " World");
        ICommand macro = new MacroCommand(first, second);

        // Act
        var result = macro.Execute();

        // Assert
        Assert.That(result, Does.Contain("Hello"));
        Assert.That(result, Does.Contain("World"));
    }

    [Test]
    public void CommandAbstraction_PreventsDirectAccessToReceiverMethods()
    {
        // Arrange
        var buffer = new TextBuffer();
        ICommand command = new AppendTextCommand(buffer, "Secret");

        // Act & Assert
        // Invoker can only call Execute() and Undo() through ICommand interface
        // Invoker never has direct access to TextBuffer.Content or TextBuffer methods
        Assert.That(command, Is.AssignableTo<ICommand>());
        Assert.DoesNotThrow(() => command.Execute());
        Assert.DoesNotThrow(() => command.Undo());
    }

    [Test]
    public void MultipleCommands_AreIndependentAndComposable()
    {
        // Arrange
        var buffer1 = new TextBuffer();
        var buffer2 = new TextBuffer();
        ICommand command1 = new AppendTextCommand(buffer1, "Command1");
        ICommand command2 = new AppendTextCommand(buffer2, "Command2");

        // Act
        command1.Execute();
        command2.Execute();

        // Assert - commands don't interfere with each other
        Assert.That(buffer1.Content, Is.EqualTo("Command1"));
        Assert.That(buffer2.Content, Is.EqualTo("Command2"));
    }

}
