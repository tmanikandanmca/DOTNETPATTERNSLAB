using Behavioral.Iterator.FailFastIterator.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => FailFastIteratorDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Iterator",
    variant = "Fail-fast Iterator",
    summary = "The iterator detects collection mutation during traversal and throws immediately."
});

app.Run();

public partial class Program;
