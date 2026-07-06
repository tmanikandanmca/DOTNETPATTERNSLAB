using Behavioral.TemplateMethod.AbstractClassTemplate.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => TemplateMethodDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Template Method",
    variant = "Pure Template Method",
    summary = "A base class fixes the full algorithm order and derived types override only the required steps."
});

app.Run();

public partial class Program;
