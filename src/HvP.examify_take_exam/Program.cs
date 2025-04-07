using HvP.Database.DBContexts;
using HvP.DB.Common.Config;
using HvP.examify_take_exam.DB.Cache;
using HvP.examify_take_exam.DB.Exceptions;
using HvP.examify_take_exam.DB.Repository.Cache;
using HvP.examify_take_exam.Services;
using StackExchange.Redis;
using Serilog;
using Serilog.Events;
using HvP.examify_take_exam.DB.Logger;
using HvP.examify_take_exam.DB.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Inject Service dependency
builder.Services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
builder.Services.AddScoped<ITakeExamService, TakeExamService>();

// Inject Repository dependency
builder.Services.AddScoped<ICacheRepository, CacheRepository>();

// Add config connect DB
builder.Services.AddScoped<CommonDBContext>(provider =>
{
    var logger = new LoggerService<PostgresDbConnection>();
    IDBConnection dbConnection = new PostgresDbConnection(EnvConfig.MasterDatabaseConfig, logger);
    return new CommonDBContext(dbConnection);
});

// Add config connect cache
builder.Services.AddSingleton<ICache>(provider =>
{
    var logger = new LoggerService<RedisConnection>();
    ConnectionMultiplexer redisConnection = new RedisConnection(EnvConfig.RedisConfig, logger).GetConnection();
    return new RedisCache(redisConnection);
});


// # Add config Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    //.Enrich.WithProperty("Application", builder.Environment.ApplicationName)
    //.Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    //.Enrich.WithMachineName()
    .Enrich.With<TraceIdEnricher>()
    .Enrich.With<MemoryUsageEnricher>()
    .Enrich.With<SourceContextShortEnricher>()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .Enrich.FromLogContext()
    //.WriteTo.File(
    //    outputTemplate: LoggerService<Program>.GetOutPutTemplate(),
    //    path: "logs/log-.json",
    //    rollingInterval: RollingInterval.Day,
    //    retainedFileCountLimit: 7)
    .WriteTo.Async(log => log.Console(
        theme: LoggerService<Program>.GetColorTheme(),
        outputTemplate: LoggerService<Program>.GetOutPutTemplate()
    ))
    .CreateLogger();


// # Config Swagger
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder
            .AllowAnyOrigin()  // Allow all domain
            .AllowAnyMethod()  // Allow all HTTP methods (GET, POST, etc.)
            .AllowAnyHeader(); // Allow all headers
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Exception handler
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Host.UseSerilog();

try
{
    string startStr = $"\u001b[36m{File.ReadAllText("start.txt")}\u001b[0m";
    Log.Information(startStr);
}
catch (Exception ex)
{
    Log.Error(ex, "start.txt NotFound");
}

var app = builder.Build();
app.UseCors("AllowAllOrigins");

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    Log.CloseAndFlush();
});

// Config Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<TraceRequestMiddleware>();

app.UseExceptionHandler(option => { });

app.MapControllers();

app.Run();
