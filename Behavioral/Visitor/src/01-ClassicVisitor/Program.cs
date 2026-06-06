using Behavioral.Visitor.ClassicVisitor.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => VisitorDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Visitor",
    variant = "Classic Visitor",
    summary = "Operations are moved into visitor objects instead of being added to the visited types."
});

app.Run();

public partial class Program;
