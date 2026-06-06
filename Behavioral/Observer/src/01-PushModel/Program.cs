using Observer.PushModel.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => ObserverDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Observer",
    variant = "Push model",
    summary = "Shows push, pull, and event-delegate notification styles in one small API sample."
});

app.Run();

public partial class Program;
