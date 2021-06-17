using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Domain.Exceptions
{
    public class NoteDomainException : Exception
    {
        public NoteDomainException() { }

        public NoteDomainException(string message) : base(message)
        {

        }

        public NoteDomainException(string message, Exception innerException) 
            : base(message, innerException)
        {

        }
    }
}
