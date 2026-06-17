using Behavioral.Memento.BlackBoxMemento.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => MementoDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Memento",
    variant = "Black-box Memento",
    summary = "State is captured and restored without exposing the internal representation."
});

app.Run();

public partial class Program;
