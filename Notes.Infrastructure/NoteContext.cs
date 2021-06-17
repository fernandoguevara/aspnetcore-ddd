using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Notes.Domain.AggregatesModel;
using Notes.Domain.SeedWork;
using Notes.Infrastructure.EntityConfigurations;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.Infrastructure
{
    public class NoteContext : DbContext, IUnitOfWork
    {
        //public const string DEFAULT_SCHEMA = "db";
        public DbSet<Note> Notes { get; set; }
        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;

        //----

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public NoteContext(DbContextOptions<NoteContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            System.Diagnostics.Debug.WriteLine("NoteContext::ctor ->" + this.GetHashCode());
        }

        protected NoteContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            System.Diagnostics.Debug.WriteLine("NoteContext::ctor ->" + this.GetHashCode());
        }

        public static readonly Microsoft.Extensions.Logging.LoggerFactory _myLoggerFactory =
        new LoggerFactory(new[] {
            new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
        });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_myLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NoteEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EmailEntityConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);
            var result = await SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            return _currentTransaction;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is IEntityDate entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreatedAt = now;
                            entity.ModifiedAt = now;
                            break;

                        case EntityState.Modified:
                            entity.ModifiedAt = now;
                            break;
                    }
                }
            }

            return base.SaveChangesAsync();
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction != null) throw new ArgumentException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        private void RollBackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;

                }

            }
        }
    }

}
