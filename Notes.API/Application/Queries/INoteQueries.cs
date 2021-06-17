using Notes.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.API.Application.Queries
{
    public interface INoteQueries
    {
        Task<NoteDTO> GetNoteAsync(Guid id);
        Task<IEnumerable<NoteDTO>> GetNotesAsync();
    }
}
