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
    summary = "Subject pushes the changed state directly to observers via an observer abstraction."
});

app.Run();

public partial class Program;
