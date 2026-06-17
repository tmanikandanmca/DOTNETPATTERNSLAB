namespace Behavioral.Command.UnitTests;

using Behavioral.Command.CompositeCommand.Api;

/// <summary>
/// Test checklist for CompositeCommand variant:
/// ✓ Command classes are executable without caller knowing receiver internals
/// ✓ Invoker code accepts abstractions (ICommand interface)
/// ✓ Composite commands preserve execution order across child commands
/// ✓ Command execution is testable in isolation with mocked receivers
/// </summary>
public class CompositeCommandChecklistTests
{
    [Test]
    public void Execute_ExecutesCompositeCommandWithoutCallerKnowingReceiverInternals()
    {
        // Arrange
        var log = new List<string>();
        ICommand command = new CompositeCommand(
            new WriteCommand(log, "Step1"),
            new WriteCommand(log, "Step2"),
            new WriteCommand(log, "Step3"));

        // Act - invoker only knows about ICommand abstraction
        var result = command.Execute();

        // Assert
        Assert.That(result, Does.Contain("Step1"));
        Assert.That(result, Does.Contain("Step2"));
        Assert.That(result, Does.Contain("Step3"));
        Assert.That(log.Count, Is.EqualTo(3));
    }

    [Test]
    public void InvokerAcceptsAbstractions_CanExecuteSimpleOrCompositeCommand()
    {
        // Arrange
        var log = new List<string>();
        ICommand simpleCommand = new WriteCommand(log, "Simple");
        ICommand compositeCommand = new CompositeCommand(
            new WriteCommand(log, "Composite1"),
            new WriteCommand(log, "Composite2"));

        // Act - invoker treats both the same way
        simpleCommand.Execute();
        compositeCommand.Execute();

        // Assert
        Assert.That(log, Contains.Item("Simple"));
        Assert.That(log, Contains.Item("Composite1"));
        Assert.That(log, Contains.Item("Composite2"));
    }

    [Test]
    public void CompositeCommand_PreservesExecutionOrderAcrossChildCommands()
    {
        // Arrange
        var executionOrder = new List<int>();
        var cmd1 = new TrackingCommand(executionOrder, 1);
        var cmd2 = new TrackingCommand(executionOrder, 2);
        var cmd3 = new TrackingCommand(executionOrder, 3);
        ICommand composite = new CompositeCommand(cmd1, cmd2, cmd3);

        // Act
        composite.Execute();

        // Assert - order must be preserved
        Assert.That(executionOrder, Is.EqualTo(new[] { 1, 2, 3 }));
    }

    [Test]
    public void CommandExecutionIsTestableInIsolation_WithRealReceiver()
    {
        // Arrange - command can be tested in isolation with real receiver
        var log = new List<string>();
        ICommand command = new WriteCommand(log, "TestData");

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(log.Count, Is.EqualTo(1));
        Assert.That(log[0], Is.EqualTo("TestData"));
    }

    [Test]
    public void NestedCompositeCommands_PreserveOrderRecursively()
    {
        // Arrange
        var executionOrder = new List<int>();
        var log1 = new List<string>();
        var log2 = new List<string>();

        var inner1 = new TrackingCommand(executionOrder, 1);
        var inner2 = new TrackingCommand(executionOrder, 2);
        ICommand innerComposite = new CompositeCommand(inner1, inner2);

        var outer1 = new TrackingCommand(executionOrder, 3);
        var outer2 = new TrackingCommand(executionOrder, 4);
        ICommand outerComposite = new CompositeCommand(outer1, innerComposite, outer2);

        // Act
        outerComposite.Execute();

        // Assert
        Assert.That(executionOrder, Is.EqualTo(new[] { 3, 1, 2, 4 }));
    }

    [Test]
    public void CompositeCommand_JoinsResultsWithSeparator()
    {
        // Arrange
        var log = new List<string>();
        ICommand composite = new CompositeCommand(
            new WriteCommand(log, "A"),
            new WriteCommand(log, "B"),
            new WriteCommand(log, "C"));

        // Act
        var result = composite.Execute();

        // Assert - results are joined with " | " separator
        Assert.That(result, Is.EqualTo("A | B | C"));
    }

    [Test]
    public void CommandAbstraction_PreventDirectAccessToReceiverState()
    {
        // Arrange
        var log = new List<string>();
        ICommand command = new CompositeCommand(
            new WriteCommand(log, "Item1"),
            new WriteCommand(log, "Item2"));

        // Act & Assert
        // Invoker can only call Execute() through ICommand interface
        // Invoker never has direct access to the log or child commands
        Assert.That(command, Is.AssignableTo<ICommand>());
        Assert.DoesNotThrow(() => command.Execute());
    }

    [Test]
    public void MultipleCompositeCommands_AreIndependent()
    {
        // Arrange
        var log1 = new List<string>();
        var log2 = new List<string>();

        ICommand composite1 = new CompositeCommand(
            new WriteCommand(log1, "Op1"),
            new WriteCommand(log1, "Op2"));

        ICommand composite2 = new CompositeCommand(
            new WriteCommand(log2, "Op3"),
            new WriteCommand(log2, "Op4"));

        // Act
        composite1.Execute();
        composite2.Execute();

        // Assert - each composite maintains its own state
        Assert.That(log1, Is.EqualTo(new[] { "Op1", "Op2" }));
        Assert.That(log2, Is.EqualTo(new[] { "Op3", "Op4" }));
    }

    [Test]
    public void EmptyCompositeCommand_ExecutesGracefully()
    {
        // Arrange
        ICommand composite = new CompositeCommand();

        // Act & Assert
        Assert.DoesNotThrow(() => composite.Execute());
    }

    /// <summary>
    /// Tracking command for verifying execution order.
    /// </summary>
    private sealed class TrackingCommand : ICommand
    {
        private readonly List<int> _executionOrder;
        private readonly int _order;

        public TrackingCommand(List<int> executionOrder, int order)
        {
            _executionOrder = executionOrder;
            _order = order;
        }

        public string Execute()
        {
            _executionOrder.Add(_order);
            return $"Step{_order}";
        }
    }

}
