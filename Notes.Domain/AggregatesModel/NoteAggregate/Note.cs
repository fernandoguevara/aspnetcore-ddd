using Notes.Domain.AggregatesModel.NoteAggregate;
using Notes.Domain.Events;
using Notes.Domain.Exceptions;
using Notes.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Notes.Domain.AggregatesModel
{
    public class Note : Entity, IAgreggateRoot
    {
        public Guid GetUserId => _userId;
        private Guid _userId;

        public string Title { get; private set; }
        public string Description { get; private set; }
        private DateTime _createdAt;
        private DateTime _modifiedAt;

        private List<Email> _emailActions;
        public IEnumerable<Email> EmailActions => _emailActions.AsReadOnly();

        protected Note() 
        {
            _emailActions = new List<Email>();
        }

        public Note(Guid userId, string title, string description) : this()
        {
            _userId = userId;
            Title = !String.IsNullOrWhiteSpace(title) ? title : throw new ArgumentException(nameof(title));
            Description = !String.IsNullOrWhiteSpace(description) ? description : throw new ArgumentException(nameof(description));
            var time = DateTime.UtcNow;

            if (IsTooLate(time)) throw new NoteDomainException("Its too late go back to sleep :(");

            _createdAt = time;
            _modifiedAt = time;
        }

        public void AddEmail(string action)
        {
            var email = new Email(action);
            _emailActions.Add(email);
            AddDomainEvent(new NoteCreatedDomainEvent(this));
        }

        public void Update(string title, string description)
        {
            Title = title;
            Description = description;
            _modifiedAt = DateTime.UtcNow;
        }

        private bool IsTooLate(DateTime now)
        {
            return now.Hour >= 12 && now.Hour < 7;
        }
    }
}
