using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.BackgroundTasks.Services
{
    public class ReportingManagerService : BackgroundService
    {
        private readonly ILogger<ReportingManagerService> _logger;
        private readonly BackgroundTaskSettings _settings;
        private readonly IReportService _reportService;

        public ReportingManagerService(IOptions<BackgroundTaskSettings> settings, 
            ILogger<ReportingManagerService> logger,
            IReportService reportService)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notes Report is starting.");

            stoppingToken.Register(() => _logger.LogInformation("#1 Notes Report background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Notes Report background task is doing background work.");

                await _reportService.CreateNotesReport();

                await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
            }

            _logger.LogInformation("Notes Report background task is stopping.");
        }
    }
}
