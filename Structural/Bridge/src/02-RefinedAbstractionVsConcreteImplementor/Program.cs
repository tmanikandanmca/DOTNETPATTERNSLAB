using Structural.Bridge.RefinedAbstractionVsConcreteImplementor.Api;

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
    pattern = "Bridge",
    variant = "Refined Abstraction vs Concrete Implementor",
    summary = "Shows refined abstractions using interchangeable implementors."
});

app.Run();

public partial class Program;
