using Autofac;
using FluentValidation;
using MediatR;
using Notes.API.Application.Behaviors;
using Notes.API.Application.Commands;
using Notes.API.Application.DomainEventHandlers.OrderCreated;
using Notes.API.Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Notes.API.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Need to check why it works with asself...
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
               .AsImplementedInterfaces().AsSelf();

            builder.RegisterAssemblyTypes(typeof(CreateNoteCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(NoteCreatedDomainEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            builder
                .RegisterAssemblyTypes(typeof(CreateNoteCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        }
    }
}
