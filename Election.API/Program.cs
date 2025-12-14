using Election.DATA;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔥 ADD LOGGING CONFIGURATION
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ADD DB CONTEXT (CONNECT TO SQL SERVER)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ ADD CORS (IMPORTANT for WinForms to call API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Allow WinForms app
              .AllowAnyMethod()    // Allow all HTTP methods
              .AllowAnyHeader();   // Allow all headers
    });
});

// ✅ ADD FILE UPLOAD SUPPORT (for manifesto and photo uploads)
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 100 * 1024 * 1024; // 100MB file size limit
});

var app = builder.Build();

// ✅ USE CORS (Must come before other middleware)
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ ENABLE STATIC FILES (to serve uploaded manifesto and photo files)
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// ✅ CREATE UPLOADS DIRECTORY IF NOT EXISTS
var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(webRootPath))
{
    Directory.CreateDirectory(webRootPath);
}

var uploadsPath = Path.Combine(webRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(Path.Combine(uploadsPath, "manifestos"));
    Directory.CreateDirectory(Path.Combine(uploadsPath, "photos"));
    Console.WriteLine("✅ Upload directories created at: " + uploadsPath);
}

Console.WriteLine("🚀 Election API Started!");
Console.WriteLine($"📁 Uploads Directory: {uploadsPath}");
Console.WriteLine($"🌐 API URL: {app.Urls}");

app.Run();