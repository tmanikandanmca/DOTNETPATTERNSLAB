using Singleton.EagerInitialization.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => EagerInitializationDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Singleton",
    variant = "Eager Initialization",
    summary = "Creates the instance immediately when the type is first loaded."
});

app.Run();

public partial class Program;
