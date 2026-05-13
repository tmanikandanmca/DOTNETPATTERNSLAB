using Structural.Proxy.VirtualProxy.Api;

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
    variant = "Virtual Proxy",
    summary = "Defers expensive object creation until first use."
});

app.Run();

public partial class Program;
