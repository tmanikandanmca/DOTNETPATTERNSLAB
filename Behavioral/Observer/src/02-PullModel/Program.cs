using Behavioral.Observer.PullModel.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => PullModelDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Observer",
    variant = "Pull Model",
    summary = "Observers read the updated subject state after the notification."
});

app.Run();

public partial class Program;
