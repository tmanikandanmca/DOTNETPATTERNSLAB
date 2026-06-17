using Behavioral.Iterator.InternalIterator.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => InternalIteratorDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Iterator",
    variant = "Internal Iterator",
    summary = "The collection controls traversal and applies a callback over each item."
});

app.Run();

public partial class Program;
