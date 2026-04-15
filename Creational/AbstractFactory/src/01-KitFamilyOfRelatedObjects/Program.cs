using AbstractFactory.KitFamilyOfRelatedObjects.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => KitFamilyOfRelatedObjectsDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "AbstractFactory",
    variant = "Kit (family of related objects)",
    summary = "Creates matching UI parts from the same family so they work together."
});

app.Run();

public partial class Program;
