using FactoryMethod.VirtualConstructor.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => VirtualConstructorDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "FactoryMethod",
    variant = "Virtual Constructor",
    summary = "Subclasses override a factory method to decide which formatter to create."
});

app.Run();

public partial class Program;
