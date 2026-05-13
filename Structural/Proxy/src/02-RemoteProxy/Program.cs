using Structural.Proxy.RemoteProxy.Api;

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
    variant = "Remote Proxy",
    summary = "Represents an object located on a remote boundary."
});

app.Run();

public partial class Program;
