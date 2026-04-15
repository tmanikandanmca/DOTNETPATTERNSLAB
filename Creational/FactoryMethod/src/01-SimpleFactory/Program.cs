using FactoryMethod.SimpleFactory.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => SimpleFactoryDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "FactoryMethod",
    variant = "Simple Factory (static)",
    summary = "A static creation method chooses a concrete product from an input key."
});

app.Run();

public partial class Program;
