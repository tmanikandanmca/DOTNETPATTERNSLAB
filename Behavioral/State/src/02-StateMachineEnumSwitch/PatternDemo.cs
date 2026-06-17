namespace Behavioral.State.StateMachineEnumSwitch.Api;

public enum WorkflowState
{
    Draft,
    Review,
    Approved,
    Published
}

public sealed class WorkflowMachine
{
    public WorkflowState State { get; private set; } = WorkflowState.Draft;

    public WorkflowState Next() => State = State switch
    {
        WorkflowState.Draft => WorkflowState.Review,
        WorkflowState.Review => WorkflowState.Approved,
        WorkflowState.Approved => WorkflowState.Published,
        _ => WorkflowState.Published
    };
}

public static class StateMachineDemo
{
    public static object Create()
    {
        var machine = new WorkflowMachine();
        var states = new List<string> { machine.State.ToString() };

        states.Add(machine.Next().ToString());
        states.Add(machine.Next().ToString());
        states.Add(machine.Next().ToString());

        return new
        {
            Pattern = "State",
            Variant = "State Machine (enum + switch)",
            States = states
        };
    }
}
