using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notes.BackgroundTasks.Queries;
using Notes.Common.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Notes.BackgroundTasks.Services
{
    public class ReportService : IReportService
    {
        private readonly IEmailService _emailService;
        private readonly BackgroundTaskSettings _settings;
        private readonly INoteQueries _noteQueries;
        private readonly ILogger<ReportingManagerService> _logger;

        public ReportService(IEmailService emailService, 
            IOptions<BackgroundTaskSettings> settings,
            INoteQueries noteQueries,
            ILogger<ReportingManagerService> logger
            )
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _noteQueries = noteQueries ?? throw new ArgumentNullException(nameof(noteQueries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task CreateNotesReport()
        {
            
            var fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".xlsx";

            _logger.LogInformation("Creating Notes Report {FileName}", fileName);

            var notes = await _noteQueries.GetNotesAsync();
            CreateExcelFile(fileName, "Notes", notes);

            _logger.LogInformation("Sending Notes Report {FileName} Email", fileName);
            await _emailService.SendEmailAsync(_settings.ReportEmail, "Report", "Notes Report", fileName);
            _logger.LogInformation("Sent Notes Report {FileName} Email", fileName);

            File.Delete(fileName);

            _logger.LogInformation("Created Notes Report {FileName}", fileName);
        }

        private void CreateExcelFile<TModel>(string fileName, string workSheet, IEnumerable<TModel> notes)
        {
            var workbook = new XLWorkbook();
            var wsDetailedData = workbook.AddWorksheet(workSheet); 
            wsDetailedData.Cell(1, 1).InsertTable(notes); 
            workbook.SaveAs(fileName); 
        }
    }
}
