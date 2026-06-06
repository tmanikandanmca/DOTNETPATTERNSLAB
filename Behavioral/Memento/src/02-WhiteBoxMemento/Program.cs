using Behavioral.Memento.WhiteBoxMemento.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => WhiteBoxMementoDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Memento",
    variant = "White-box Memento",
    summary = "The snapshot structure is exposed and can be inspected by the caretaker."
});

app.Run();

public partial class Program;
