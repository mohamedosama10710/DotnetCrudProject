using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/app-log-.txt", rollingInterval: RollingInterval.Day) // ينشئ ملفاً جديداً كل يوم تلقائياً
    .CreateLogger();
    
builder.Services.AddControllers();


builder.Host.UseSerilog();
builder.Services.AddHttpLogging(options => { });

builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWhen(
    // (الشرط): هل مسار الطلب يبدأ بـ "/api/products"؟
    context => context.Request.Path.StartsWithSegments("/api/products"),
    appBuilder =>
    {
        appBuilder.Use(async (context, next) =>
        {
            Console.WriteLine($"[Custom UseWhen Middleware] Request intercepted for path: {context.Request.Path}");
            await next();
        });
    }
    
 
);
app.MapControllers();
 Task.Run(()=>app.RunAsync());

// await Task.Delay(2000);
// await  ClientGenerator.GenerateClientCode();