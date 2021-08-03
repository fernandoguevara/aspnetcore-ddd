using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notes.API.Application.Commands;
using Notes.API.Application.Queries;
using Notes.API.Application.Requests;
using Notes.API.Application.Services;
using Notes.API.Extensions;
using Notes.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Notes.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly INoteQueries _noteQueries;
        private readonly IIdentityService _identityService;
        private readonly ILogger<NotesController> _logger;
        private readonly INoteRepository _noteRepository;

        public NotesController(IMediator mediator, INoteQueries noteQueries,
            IIdentityService identityService, ILogger<NotesController> logger,
            INoteRepository noteRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _noteQueries = noteQueries ?? throw new ArgumentNullException(nameof(noteQueries));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
            _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NoteDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> GetNotesAsync()
        {
            var notes = await _noteQueries.GetNotesAsync();

            return Ok(notes);
        }

        [Authorize(Roles = "user")]
        [Route("{noteId:guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(NoteDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> GetNoteAsync(Guid noteId)
        {
            var note = await _noteQueries.GetNoteAsync(noteId);

            return Ok(note);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(typeof(NoteDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<NoteDTO>> CreateNoteAsync([FromBody] CreateNoteCommand command)
        {
            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                command.GetGenericTypeName(),
                nameof(command.Title),
                command.Title,
                command);

            return await _mediator.Send(command);
        }
        

        //is like the other post above but it does not use mediator or domains, you know simpler... :P
        [Authorize(Roles = "admin")]
        [HttpPost("simpler")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> SimplerCreateNoteAsync([FromBody] CreateNoteCommand command)
        {
            var userId = _identityService.GetUserIdentity();
            var note = new Note(Guid.Parse(userId), command.Title, command.Description);
            note.AddEmail("Created Email");
            _noteRepository.Add(note);
            var result = await _noteRepository.UnitOfWork.SaveChangesAsync();

            if(result < 1)
            {
                return BadRequest();
            }

            return Ok("worked? !!!! simpler????");

        }

        [Authorize(Roles = "admin")]
        [Route("{noteId:guid}")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateNoteAsync([FromBody] UpdateNoteRequest request, Guid noteId)
        {
            var note = await _noteRepository.FindByIdAsync(noteId);
            note.Update(request.Title, request.Description);
            _noteRepository.Update(note);
            var result = await _noteRepository.UnitOfWork.SaveChangesAsync();

            if (result < 1)
            {
                return BadRequest();
            }

            return Ok("updated? :D");
        }

        [Authorize(Roles = "admin")]
        [Route("{noteId:guid}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteNoteAsync(Guid noteId)
        {
            var note = await _noteRepository.FindByIdAsync(noteId);
            note.AddEmail("Email Deleted");
            _noteRepository.Delete(note);
            var result = await _noteRepository.UnitOfWork.SaveChangesAsync();

            if (result < 1)
            {
                return BadRequest();
            }

            return Ok("gone? D:");
        }
    }
}
