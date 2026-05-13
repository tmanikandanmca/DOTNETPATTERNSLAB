using Structural.Composite.SafeComposite.Api;

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
    pattern = "Composite",
    variant = "Safe Composite",
    summary = "Restricts child-management operations to composite nodes only."
});

app.Run();

public partial class Program;
