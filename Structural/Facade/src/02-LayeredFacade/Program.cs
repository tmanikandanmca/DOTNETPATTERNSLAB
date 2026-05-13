using Structural.Facade.LayeredFacade.Api;

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
    pattern = "Facade",
    variant = "Layered Facade",
    summary = "Stacks facades by layer to keep responsibilities separated."
});

app.Run();

public partial class Program;
