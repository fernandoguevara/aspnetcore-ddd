using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.API.Application.Requests
{
    public class UpdateNoteRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
