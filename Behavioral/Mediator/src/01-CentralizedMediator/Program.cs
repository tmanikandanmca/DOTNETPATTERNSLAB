using Behavioral.Mediator.CentralizedMediator.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => MediatorDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Mediator",
    variant = "Centralized Mediator",
    summary = "A single mediator routes communication between colleagues and keeps them decoupled."
});

app.Run();

public partial class Program;
