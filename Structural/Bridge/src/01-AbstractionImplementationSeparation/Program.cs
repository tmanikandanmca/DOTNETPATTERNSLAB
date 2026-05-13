using Structural.Bridge.AbstractionImplementationSeparation.Api;

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
    variant = "Abstraction-Implementation Separation",
    summary = "Decouples abstraction from implementation so both evolve independently."
});

app.Run();

public partial class Program;
