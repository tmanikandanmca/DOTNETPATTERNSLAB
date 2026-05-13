using Structural.Decorator.DynamicVsStaticDecoration.Api;

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
    pattern = "Decorator",
    variant = "Dynamic vs Static Decoration",
    summary = "Compares runtime wrapping vs fixed, compile-time decoration."
});

app.Run();

public partial class Program;
