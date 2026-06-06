using Behavioral.Visitor.AcyclicVisitor.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => AcyclicVisitorDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Visitor",
    variant = "Acyclic Visitor",
    summary = "Visitor capabilities are split by role interfaces to reduce dependency cycles."
});

app.Run();

public partial class Program;
