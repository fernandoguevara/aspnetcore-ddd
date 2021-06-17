using Autofac;
using MediatR;
using Notes.API.Application.Commands;
using Notes.API.Application.Queries;
using Notes.Domain.AggregatesModel;
using Notes.Infrastructure.Idempotency;
using Notes.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Notes.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string queriesConnectionString)
        {
            QueriesConnectionString = queriesConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new NoteQueries(QueriesConnectionString))
                .As<INoteQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<NoteRepository>()
                .As<INoteRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestManager>()
               .As<IRequestManager>()
               .InstancePerLifetimeScope();

        }
    }
}
