using HvP.Database.DBContexts;
using HvP.DB.Common.Config;
using HvP.examify_take_exam.DB.Cache;
using HvP.examify_take_exam.DB.Exceptions;
using HvP.examify_take_exam.DB.Repository.Cache;
using HvP.examify_take_exam.Services;
using StackExchange.Redis;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Inject Service dependency
builder.Services.AddScoped<ITakeExamService, TakeExamService>();

// Inject Repository dependency
builder.Services.AddScoped<ICacheRepository, CacheRepository>();

// Add config connect DB
builder.Services.AddScoped<CommonDBContext>(provider =>
{
    IDBConnection dbConnection = new PostgresDbConnection(EnvConfig.MasterDatabaseConfig);
    return new CommonDBContext(dbConnection);
});

// Add config connect cache
builder.Services.AddScoped<ICache>(provider =>
{
    ConnectionMultiplexer redisConnection = new RedisConnection(EnvConfig.RedisConfig).GetConnection();
    return new RedisCache(redisConnection);
});


// Add config Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .CreateLogger();

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
    string startStr = File.ReadAllText("start.txt");
    Log.Information(startStr);
}
catch (Exception ex)
{
    Log.Error(ex, "start.txt NotFound");
}

var app = builder.Build();

// Config Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler(option => { });

app.MapControllers();

app.Run();
