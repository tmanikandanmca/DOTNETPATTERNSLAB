using FactoryMethod.ParameterizedFactory.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => ParameterizedFactoryDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "FactoryMethod",
    variant = "Parameterized Factory",
    summary = "Factory behavior changes based on parameters such as channel and priority."
});

app.Run();

public partial class Program;
