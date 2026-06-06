using Behavioral.TemplateMethod.HookMethods.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => HookMethodsDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Template Method",
    variant = "Hook Methods",
    summary = "Optional extension points customize steps without changing the template skeleton."
});

app.Run();

public partial class Program;
