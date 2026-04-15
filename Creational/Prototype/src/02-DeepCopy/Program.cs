using Prototype.DeepCopy.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => DeepCopyDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Prototype",
    variant = "Deep Copy",
    summary = "Clones the full object graph so nested data is independent."
});

app.Run();

public partial class Program;
