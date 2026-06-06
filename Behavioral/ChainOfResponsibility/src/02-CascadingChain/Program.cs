using Behavioral.ChainOfResponsibility.CascadingChain.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => CascadingChainDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Chain of Responsibility",
    variant = "Cascading Chain",
    summary = "Multiple handlers contribute to the final result in sequence."
});

app.Run();

public partial class Program;
