using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.Infrastructure
{
    public class SQLServerNoteContext : NoteContext
    {
        public SQLServerNoteContext(DbContextOptions<SQLServerNoteContext> options, IMediator mediator)
            : base(options, mediator) { }
    }

    public class SQLServerNoteContextDesignFactory : IDesignTimeDbContextFactory<SQLServerNoteContext>
    {
        public SQLServerNoteContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SQLServerNoteContext>()
                .UseSqlServer();

            return new SQLServerNoteContext(optionsBuilder.Options, new NoMediator());
        }

        class NoMediator : IMediator
        {
            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<TResponse>(default(TResponse));
            }

            public Task<object> Send(object request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(default(object));
            }
        }
    }
}
