using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Notes.API.Extensions;
using Notes.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Notes.Test
{
    public class NotesScenarioBase
    {
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(NotesScenarioBase))
              .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("Notes.API/appsettings.json", optional: false)
                    .AddEnvironmentVariables();
                }).UseStartup<NotesTestsStartup>();

            var testServer = new TestServer(hostBuilder);

            testServer.Host
                .MigrateDbContext<NoteContext>((context, services) =>
                {
                });

            return testServer;
        }
    }
}
