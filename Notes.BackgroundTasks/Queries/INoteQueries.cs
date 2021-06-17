using Notes.BackgroundTasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.BackgroundTasks.Queries
{
    public interface INoteQueries
    {
        Task<IEnumerable<Note>> GetNotesAsync();
    }
}
