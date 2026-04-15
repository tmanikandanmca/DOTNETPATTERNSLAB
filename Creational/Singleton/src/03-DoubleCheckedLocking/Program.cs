using Singleton.DoubleCheckedLocking.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => DoubleCheckedLockingDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Singleton",
    variant = "Double-Checked Locking",
    summary = "Reduces locking overhead by checking the instance twice."
});

app.Run();

public partial class Program;
