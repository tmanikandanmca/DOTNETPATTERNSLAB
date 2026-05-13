using Structural.Proxy.SmartProxy.Api;

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
    pattern = "Proxy",
    variant = "Smart Proxy",
    summary = "Adds cross-cutting behavior like caching and metrics."
});

app.Run();

public partial class Program;
