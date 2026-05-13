using Structural.Composite.TransparentComposite.Api;

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
    variant = "Transparent Composite",
    summary = "Exposes a uniform interface for both leaf and composite nodes."
});

app.Run();

public partial class Program;
