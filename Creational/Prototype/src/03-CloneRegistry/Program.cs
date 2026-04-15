using Prototype.CloneRegistry.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => CloneRegistryDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Prototype",
    variant = "Clone Registry",
    summary = "Stores ready-made prototypes and clones them by key when needed."
});

app.Run();

public partial class Program;
