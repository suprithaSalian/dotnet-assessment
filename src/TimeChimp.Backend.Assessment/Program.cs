using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;
using TimeChimp.Backend.Assessment.Repository;

namespace TimeChimp.Backend.Assessment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using AsyncServiceScope asyncScope = host.Services.CreateAsyncScope();
            var _dbContext = asyncScope.ServiceProvider.GetRequiredService<RssDbContext>();
            _dbContext.Database.EnsureCreated();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                        {
                            // Set properties and call methods on options
                        })
                        .UseSerilog((context, config) =>
                        {
                            config.ReadFrom.Configuration(context.Configuration)
                                  .WriteTo.Console(new JsonFormatter(renderMessage: true))
                                  .Enrich.FromLogContext();
                        })
                        .UseStartup<Startup>();
                });
    }
}