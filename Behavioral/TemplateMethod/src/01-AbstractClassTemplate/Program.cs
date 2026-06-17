using Behavioral.TemplateMethod.AbstractClassTemplate.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => TemplateMethodDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Template Method",
    variant = "Abstract Class Template",
    summary = "A base class defines the algorithm skeleton and derived types fill in the steps."
});

app.Run();

public partial class Program;
