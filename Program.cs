using Adviser.Services;

var builder = WebApplication.CreateBuilder(args);

string logFilePath = builder.Configuration["LogFilePath"] ?? throw new Exception("Caminho do log n�o definido");
string uploadFilePath = builder.Configuration["UploadFilePath"] ?? throw new Exception("Caminho de upload n�o definido");

if (!Directory.Exists(logFilePath))
{
    Directory.CreateDirectory(logFilePath);
}
if (!Directory.Exists(uploadFilePath))
{
    Directory.CreateDirectory(uploadFilePath);
}

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
