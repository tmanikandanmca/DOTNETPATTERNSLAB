using Builder.DirectorBasedBuilder.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => DirectorBasedBuilderDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Builder",
    variant = "Director-based Builder",
    summary = "A director coordinates the builder to create known object recipes."
});

app.Run();

public partial class Program;
