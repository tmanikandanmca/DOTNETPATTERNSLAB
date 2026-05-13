using Structural.Decorator.TransparentDecorator.Api;

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
    variant = "Transparent Decorator",
    summary = "Adds behavior while preserving the same component interface."
});

app.Run();

public partial class Program;
