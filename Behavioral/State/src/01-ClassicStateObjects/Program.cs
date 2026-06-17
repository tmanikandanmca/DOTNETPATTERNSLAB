using Behavioral.State.ClassicStateObjects.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => StateDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "State",
    variant = "Classic State Objects",
    summary = "Behavior changes as the context swaps between dedicated state objects."
});

app.Run();

public partial class Program;
