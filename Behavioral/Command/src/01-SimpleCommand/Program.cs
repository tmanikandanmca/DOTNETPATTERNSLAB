using Behavioral.Command.SimpleCommand.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => CommandDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Command",
    variant = "Simple Command",
    summary = "A request is wrapped as a command object and executed by an invoker."
});

app.Run();

public partial class Program;
