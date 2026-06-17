using Behavioral.Command.UndoableCommand.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => UndoableCommandDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Command",
    variant = "Undoable Command",
    summary = "Each command knows how to reverse its own change."
});

app.Run();

public partial class Program;
