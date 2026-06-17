using Behavioral.Interpreter.ContextDrivenInterpreter.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => ContextDrivenInterpreterDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Interpreter",
    variant = "Context-driven Interpreter",
    summary = "Interpretation depends on runtime context values and named rules."
});

app.Run();

public partial class Program;
