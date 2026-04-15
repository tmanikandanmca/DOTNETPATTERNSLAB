using Prototype.ShallowCopy.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => ShallowCopyDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Prototype",
    variant = "Shallow Copy",
    summary = "Copies the outer object while keeping nested references shared."
});

app.Run();

public partial class Program;
