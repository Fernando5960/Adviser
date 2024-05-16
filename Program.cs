using Adviser.Services;

var builder = WebApplication.CreateBuilder(args);

string logFilePath = builder.Configuration["LogFilePath"] ?? throw new Exception("Caminho do log não definido");
logFilePath += DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".log";
builder.Services.AddControllersWithViews();
builder.Services.AddLogging(builder =>
{
    builder.AddProvider(new CustomFileLoggerProvider(File.AppendText(logFilePath)));
});

var app = builder.Build();

// app.MapGet("/", () => "Hello World!");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
