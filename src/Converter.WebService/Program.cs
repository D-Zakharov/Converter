using Converter.Domain.Config;
using Converter.Domain.DB;
using Converter.Domain.Services;
using Converter.WebService.Endpoints;
using Converter.WebService.ExceptionsHandling;
using Converter.WebService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
var loggerConfig = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Logging.AddSerilog(loggerConfig);

builder.Services.AddDbContextFactory<ConverterDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetValue<string>("ConnectionString"),
        serverDbContextOptionsBuilder =>
        {
            serverDbContextOptionsBuilder.EnableRetryOnFailure();
        });
});

builder.Services.Configure<StorageConfig>(builder.Configuration.GetSection(nameof(StorageConfig)));
builder.Services.Configure<RabbitConfiguration>(builder.Configuration.GetSection(nameof(RabbitConfiguration)));

builder.Services.AddSingleton<IRabbitConnectionManager, RabbitConnectionManager>();

builder.Services.AddScoped<IRequestData, RequestData>();
builder.Services.AddScoped<IDataStorage, DiskDataStorage>();
builder.Services.AddScoped<IQueueManager, RabbitQueueManager>();

builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.WithOrigins("*");
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.RegisterExceptionHandler();
app.MapEndpoints();

app.UseCors(builder => builder
 .AllowAnyOrigin()
 .AllowAnyMethod()
 .AllowAnyHeader());

app.Run();
