using Structural.Adapter.ClassAdapter.Api;

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
    pattern = "Adapter",
    variant = "Class Adapter",
    summary = "Adapts a legacy API using inheritance."
});

app.Run();

public partial class Program;
