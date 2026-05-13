using Structural.Proxy.ProtectionProxy.Api;

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
    pattern = "Proxy",
    variant = "Protection Proxy",
    summary = "Controls access to an object based on permissions."
});

app.Run();

public partial class Program;
