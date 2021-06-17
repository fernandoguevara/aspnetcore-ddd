using MediatR;
using Notes.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Domain.Events
{
    public class NoteCreatedDomainEvent : INotification
    {
        public Note Note { get; private set; }

        public NoteCreatedDomainEvent(Note note)
        {
            Note = note;
        }
    }
}
