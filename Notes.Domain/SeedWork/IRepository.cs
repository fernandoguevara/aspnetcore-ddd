using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Domain.SeedWork
{
    public interface IRepository<T> where T : IAgreggateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
