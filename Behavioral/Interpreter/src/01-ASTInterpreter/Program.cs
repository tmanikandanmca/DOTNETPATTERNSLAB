using Behavioral.Interpreter.ASTInterpreter.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => InterpreterDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Interpreter",
    variant = "AST Interpreter",
    summary = "A small expression tree interprets a rule through composable nodes."
});

app.Run();

public partial class Program;
