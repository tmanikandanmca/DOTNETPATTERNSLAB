using Singleton.BasicSingleton.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => BasicSingletonDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Singleton",
    variant = "Basic Singleton",
    summary = "Single shared instance with a private constructor and static accessor."
});

app.Run();

public partial class Program;
