using Structural.Flyweight.IntrinsicVsExtrinsicState.Api;

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
    pattern = "Flyweight",
    variant = "Intrinsic vs Extrinsic State",
    summary = "Shares intrinsic state and externalizes context-specific extrinsic state."
});

app.Run();

public partial class Program;
