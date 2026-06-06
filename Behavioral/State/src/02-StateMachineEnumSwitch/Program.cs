using Behavioral.State.StateMachineEnumSwitch.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => StateMachineDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "State",
    variant = "State Machine (enum + switch)",
    summary = "Transition logic is centralized in a switch expression over enum states."
});

app.Run();

public partial class Program;
