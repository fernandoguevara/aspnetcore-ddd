using Notes.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.API.Application.Queries
{
    public class NoteDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public static NoteDTO FromNote(Note note)
        {
            return new NoteDTO
            {
                Id = note.Id,
                UserId = note.GetUserId,
                Title = note.Title,
                Description = note.Description
            };
        }
    }
}
