using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notes.API.Extensions;
using Notes.Domain.SeedWork;
using Notes.Infrastructure;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Notes.API
{
    public class Program
    {
        public static string Namespace = typeof(Startup).Namespace;
        public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static void Main(string[] args)
        {

            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
                var host = BuildWebHost(configuration, args);

                Log.Information("Applying migrations ({ApplicationContext})...", Program.AppName);

                host.MigrateDbContext<NoteContext>((context, services) =>
                {
                    //I dont use seeding... yet...
                    /*var env = services.GetService<IWebHostEnvironment>();
                    var settings = services.GetService<IOptions<OrderingSettings>>();
                    var logger = services.GetService<ILogger<OrderingContextSeed>>();

                    new OrderingContextSeed()
                        .SeedAsync(context, env, settings, logger)
                        .Wait();*/
                });

                Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
                host.Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            if (config.GetValue<Boolean>("UseVault", false))
            {
                builder.AddAzureKeyVault(
                    $"https://{config["Vault:Name"]}.vault.azure.net/",
                    config["Vault:ClientId"],
                    config["Vault:ClientSecret"]);
            }

            return builder.Build();
        }

        static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
            .CaptureStartupErrors(false)
            .ConfigureKestrel(options =>
            {
                var ports = GetDefinedPorts(configuration);
                options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });

                options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });

            })
            .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
            .UseStartup<Startup>()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseSerilog()
            .Build();
        }

        static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var grpcPort = config.GetValue("GRPC_PORT", 5001);
            var port = config.GetValue("PORT", 80);
            return (port, grpcPort);
        }

        static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashgUrl"];
            var elasticSearchUrl = configuration["ElasticSearchUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", Program.AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
                })
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
