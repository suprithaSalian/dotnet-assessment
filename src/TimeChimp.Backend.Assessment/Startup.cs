using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Xml;
using TimeChimp.Backend.Assessment.Repository;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using TimeChimp.Backend.Assessment.Services;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace TimeChimp.Backend.Assessment
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        private const string RssFeedApiCorsPolicy = "_rssFeedApiCorsPolicy";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApi(Configuration);
            services.AddServices();
            services.AddCors(options =>
            {
                options.AddPolicy(RssFeedApiCorsPolicy, builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
            services.AddServiceProviders();
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new XmlContentSerializer(
                     new XmlContentSerializerSettings
                {
                        XmlReaderWriterSettings = new XmlReaderWriterSettings
                        {
                            WriterSettings = new XmlWriterSettings
                            {
                                OmitXmlDeclaration = true
                            }
                        }
                })
            };
            services.AddRefitClient<INuApi>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new System.Uri("https://www.nu.nl"))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    UseDefaultCredentials = true
                });
            services.AddScoped<IRssFeedService, RssFeedService>();
            services.AddScoped<IRssFeedRepository, RssFeedRepository>();
            services.AddDbContext<RssDbContext>(
                o=>o.UseSqlServer(Configuration.GetConnectionString("RssDb"),
                o=>o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));
            services.AddSingleton<RssReader>();
            services.AddHostedService(provider => provider.GetRequiredService<RssReader>());

        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            app.UseApi(Configuration, Environment);
            app.UseCors(RssFeedApiCorsPolicy);
            logger.LogInformation(default(EventId), $"{Assembly.GetExecutingAssembly().GetName().Name} started");
        }
    }
}
