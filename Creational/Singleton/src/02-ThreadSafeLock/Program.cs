using Singleton.ThreadSafeLock.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => ThreadSafeLockDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Singleton",
    variant = "Thread-Safe (lock)",
    summary = "Uses a lock to safely create the singleton in concurrent scenarios."
});

app.Run();

public partial class Program;
