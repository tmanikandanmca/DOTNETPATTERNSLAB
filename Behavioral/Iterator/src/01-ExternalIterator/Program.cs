using Behavioral.Iterator.ExternalIterator.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => IteratorDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Iterator",
    variant = "External Iterator",
    summary = "The caller drives iteration explicitly instead of relying on internal traversal."
});

app.Run();

public partial class Program;
