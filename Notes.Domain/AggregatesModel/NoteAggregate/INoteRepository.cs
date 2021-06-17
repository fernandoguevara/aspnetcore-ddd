using Notes.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Domain.AggregatesModel
{
    public interface INoteRepository : IRepository<Note>
    {
        Note Add(Note note);
        void Delete(Note note);
        void Update(Note note);
        Task<Note> FindByIdAsync(Guid id);
    }
}
