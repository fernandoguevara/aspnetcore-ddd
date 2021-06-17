using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Notes.API.Application.Services;
using Notes.Domain.Events;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.API.Application.DomainEventHandlers.OrderCreated
{
    public class NoteCreatedDomainEventHandler : INotificationHandler<NoteCreatedDomainEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILoggerFactory _logger;
        private readonly IIdentityService _identityService;

        public NoteCreatedDomainEventHandler(
            IEmailService emailService, 
            ILoggerFactory logger,
            IIdentityService identityService
            )
        {
            _emailService = emailService;
            _logger = logger;
            _identityService = identityService;
        }

        public async Task Handle(NoteCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<NoteCreatedDomainEvent>()
                .LogTrace("Note with Id: {NoteId} has been successfully created, sending email"
                , notification.Note.Id);

            var email = _identityService.GetUserEmail();

            //comment out this only is you set the smtp variables with a valid email account
            //await _emailService.SendEmailAsync(email, "Note Created", "You've created a new Note!");

            _logger.CreateLogger<NoteCreatedDomainEvent>()
                .LogTrace("Email Sent for Note: {NoteId}"
                , notification.Note.Id);
        }
    }
}
