using Structural.Facade.SimpleFacade.Api;

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
    pattern = "Facade",
    variant = "Simple Facade",
    summary = "Provides one simple entry point over multiple subsystems."
});

app.Run();

public partial class Program;
