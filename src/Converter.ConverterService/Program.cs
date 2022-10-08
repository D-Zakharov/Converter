using Converter.ConverterService.Services.Converters;
using Converter.Domain.Config;
using Converter.Domain.DB;
using Converter.Domain.Services;
using Converter.Domain.Services.Factories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Converter.ConverterService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var converter = new HtmlToPdfConverter();
            await converter.Init(); //доступна лишь асинхронная инициализация, в рамках конструктора она не должна вызываться

            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    services.AddDbContextFactory<ConverterDbContext>(options =>
                    {
                        options.UseSqlServer(
                            configuration.GetValue<string>("ConnectionString"),
                            serverDbContextOptionsBuilder =>
                            {
                                serverDbContextOptionsBuilder.EnableRetryOnFailure();
                            });
                    });

                    services.Configure<RabbitConfiguration>(configuration.GetSection(nameof(RabbitConfiguration)));
                    services.Configure<StorageConfig>(configuration.GetSection(nameof(StorageConfig)));

                    services.AddSingleton<IRabbitConnectionManager, RabbitConnectionManager>();
                    services.AddSingleton<IDataStorageFactory, DataStorageFactory>();
                    services.AddSingleton<IRequestDataFactory, RequestDataFactory>();

                    services.AddSingleton<IConverter>(converter);

                    //services.AddSingleton<IConverter, HtmlToPdfConverter>();

                    services.AddHostedService<Worker>();
                })
                .UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration))
                .Build();

            await host.RunAsync();
        }
    }
}