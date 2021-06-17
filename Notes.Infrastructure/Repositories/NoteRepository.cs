using Microsoft.EntityFrameworkCore;
using Notes.Domain.AggregatesModel;
using Notes.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly NoteContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public NoteRepository(NoteContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Note Add(Note note)
        {
            return _context.Notes
                    .Add(note)
                    .Entity;
        }

        public void Update(Note note)
        {
            _context.Update(note);
        }

        public void Delete(Note note)
        {
            _context.Remove(note);
        }

        public async Task<Note> FindByIdAsync(Guid id)
        {
            var note = await _context.Notes
                    .FirstAsync(b => b.Id == id);

            return note;
        }
    }
}
