using Builder.StepBuilder.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => StepBuilderDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Builder",
    variant = "Step Builder",
    summary = "Enforces required construction steps through staged interfaces."
});

app.Run();

public partial class Program;
