using Structural.Decorator.SemiTransparentDecorator.Api;

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
    variant = "Semi-transparent Decorator",
    summary = "Adds behavior and exposes limited decorator-specific capabilities."
});

app.Run();

public partial class Program;
