using System.Reflection;
using Behavioral.State.StateMachineEnumSwitch.Api;

namespace Behavioral.State.Tests;

public class StateMachineEnumSwitchChecklistTests
{
    [Test]
    public void Next_AdvancesThroughTheDeclaredStates_InOrder()
    {
        var machine = new WorkflowMachine();

        var states = new List<WorkflowState>
        {
            machine.State,
            machine.Next(),
            machine.Next(),
            machine.Next()
        };

        Assert.That(states, Is.EqualTo(new[]
        {
            WorkflowState.Draft,
            WorkflowState.Review,
            WorkflowState.Approved,
            WorkflowState.Published
        }));
    }

    [Test]
    public void Next_StaysAtTheTerminalState_AfterTheFinalTransition()
    {
        var machine = new WorkflowMachine();

        machine.Next();
        machine.Next();
        machine.Next();

        var repeatedState = machine.Next();

        Assert.That(repeatedState, Is.EqualTo(WorkflowState.Published));
    }

    [Test]
    public void UnexpectedStateValues_AreHandledClearly_ByTheFallbackBranch()
    {
        var machine = new WorkflowMachine();
        SetWorkflowState(machine, (WorkflowState)999);

        var result = machine.Next();

        Assert.That(result, Is.EqualTo(WorkflowState.Published));
    }

    [Test]
    public void Callers_OnlyNeedTheMachineAndTheEnum_StateRemainsSimpleToUse()
    {
        var machine = new WorkflowMachine();

        Assert.Multiple(() =>
        {
            Assert.That(machine.State, Is.EqualTo(WorkflowState.Draft));
            Assert.That(machine.Next(), Is.EqualTo(WorkflowState.Review));
        });
    }

    private static void SetWorkflowState(WorkflowMachine machine, WorkflowState state)
    {
        var property = typeof(WorkflowMachine).GetProperty(
            nameof(WorkflowMachine.State),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        var setter = property!.GetSetMethod(true);
        setter!.Invoke(machine, new object[] { state });
    }
}