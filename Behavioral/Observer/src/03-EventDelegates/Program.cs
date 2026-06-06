using Behavioral.Observer.EventDelegates.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => EventDelegatesDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Observer",
    variant = "Event Delegates",
    summary = "C# events expose a concise idiomatic observer implementation."
});

app.Run();

public partial class Program;
