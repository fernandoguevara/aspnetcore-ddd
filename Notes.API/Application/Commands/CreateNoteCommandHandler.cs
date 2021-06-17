using MediatR;
using Microsoft.Extensions.Logging;
using Notes.API.Application.Queries;
using Notes.API.Application.Services;
using Notes.Domain.AggregatesModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.API.Application.Commands
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, NoteDTO>
    {
        private readonly INoteRepository _noteRepository;
        private readonly ILogger<CreateNoteCommandHandler> _logger;
        private readonly IIdentityService _identityService;


        public CreateNoteCommandHandler(INoteRepository noteRepository, 
            ILogger<CreateNoteCommandHandler> logger,
            IIdentityService identityService)
        {
            _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }
        
        public async Task<NoteDTO> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            var userId = _identityService.GetUserIdentity();
            var note = new Note(Guid.Parse(userId), request.Title, request.Description);
            note.AddEmail("Note Created");

            _logger.LogInformation("----- Creating Note - Note: {@Order}", note);

            var result = _noteRepository.Add(note);

            await _noteRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return await Task.FromResult(NoteDTO.FromNote(result));
        }
    }
}
