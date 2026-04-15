using Singleton.LazyInitialization.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => LazyInitializationDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Singleton",
    variant = "Lazy Initialization (Lazy<T> in .NET)",
    summary = "Delegates delayed construction to Lazy<T> in .NET."
});

app.Run();

public partial class Program;
