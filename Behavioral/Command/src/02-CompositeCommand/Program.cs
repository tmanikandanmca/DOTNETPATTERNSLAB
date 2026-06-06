using Behavioral.Command.CompositeCommand.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => CompositeCommandDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Command",
    variant = "Composite Command",
    summary = "A macro command executes a list of child commands as one unit."
});

app.Run();

public partial class Program;
