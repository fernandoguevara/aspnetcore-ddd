using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.BackgroundTasks.Services
{
    public interface IReportService
    {
        Task CreateNotesReport();
    }
}
