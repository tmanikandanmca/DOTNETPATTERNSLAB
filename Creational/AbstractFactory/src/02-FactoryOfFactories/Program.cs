using AbstractFactory.FactoryOfFactories.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => FactoryOfFactoriesDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "AbstractFactory",
    variant = "Factory of Factories",
    summary = "A higher-level selector returns one of several concrete factories."
});

app.Run();

public partial class Program;
