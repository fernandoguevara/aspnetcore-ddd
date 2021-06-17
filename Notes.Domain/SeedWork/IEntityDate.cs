using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Domain.SeedWork
{
    public interface IEntityDate
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
