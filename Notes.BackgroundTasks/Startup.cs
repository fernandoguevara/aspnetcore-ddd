using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.BackgroundTasks.Queries;
using Notes.BackgroundTasks.Services;
using Notes.Common.Email;

namespace Notes.BackgroundTasks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            CustomMethods.SetDatabaseConnection(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                //.AddCustomHealthCheck(this.Configuration)
                .Configure<BackgroundTaskSettings>(this.Configuration)
                .AddOptions()
                .AddHostedService<ReportingManagerService>();

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<INoteQueries, NoteQueries>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseRouting();
        }
    }

    public static class CustomMethods
    {
        public static void SetDatabaseConnection(IConfiguration configuration)
        {
            configuration["ConnectionString"] = configuration["DatabaseVendor"] == "SQLServer"
                                                ? configuration["SQLServerConnectionString"]
                                                : configuration["PostgresConnectionString"];
        }
    }
}
