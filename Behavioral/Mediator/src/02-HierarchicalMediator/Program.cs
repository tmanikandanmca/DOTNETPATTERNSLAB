using Behavioral.Mediator.HierarchicalMediator.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => HierarchicalMediatorDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Mediator",
    variant = "Hierarchical Mediator",
    summary = "Messages are coordinated through team mediators and then escalated to a parent mediator."
});

app.Run();

public partial class Program;
