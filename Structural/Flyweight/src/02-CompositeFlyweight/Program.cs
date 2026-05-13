using Structural.Flyweight.CompositeFlyweight.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => PatternDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Flyweight",
    variant = "Composite Flyweight",
    summary = "Composes multiple flyweights while still reusing shared intrinsic state."
});

app.Run();

public partial class Program;
