using MediatR;
using Notes.API.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Notes.API.Application.Commands
{
    public class CreateNoteCommand : IRequest<NoteDTO>
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
