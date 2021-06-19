using Notes.Domain.Exceptions;
using Notes.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Domain.AggregatesModel.NoteAggregate
{
    public class Email : Entity
    {
        private string _action;
        private DateTime _createdAt;

        protected Email()
        {

        }

        public Email(string action) : this()
        {
            _action = !string.IsNullOrWhiteSpace(action) ? action : throw new NoteDomainException(nameof(action));
            _createdAt = DateTime.UtcNow;
        }
    }
}
