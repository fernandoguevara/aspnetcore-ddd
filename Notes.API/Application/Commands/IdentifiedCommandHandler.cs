using MediatR;
using Microsoft.Extensions.Logging;
using Notes.API.Extensions;
using Notes.Infrastructure.Idempotency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.API.Application.Commands
{
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
        where T : IRequest<R>
    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;
        private readonly ILogger<IdentifiedCommandHandler<T, R>> _logger;

        public IdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<T, R>> logger)
        {
            _mediator = mediator;
            _requestManager = requestManager;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates the result value to return if a previous request was found
        /// </summary>
        /// <returns></returns>
        protected virtual R CreateResultForDuplicateRequest()
        {
            return default(R);
        }

        /// <summary>
        /// This method handles the command. It just ensures that no other request exists with the same ID, and if this is the case
        /// just enqueues the original inner command.
        /// </summary>
        /// <param name="message">IdentifiedCommand which contains both original command & request ID</param>
        /// <returns>Return value of inner command or default value if request same ID was found</returns>
        public async Task<R> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistAsync(message.Id);
            if (alreadyExists)
            {
                return CreateResultForDuplicateRequest();
            }
            else
            {
                await _requestManager.CreateRequestForCommandAsync<T>(message.Id);
                try
                {
                    var command = message.Command;
                    var commandName = command.GetGenericTypeName();
                    var idProperty = string.Empty;
                    var commandId = string.Empty;

                    switch (command)
                    {
                        case CreateNoteCommand createNoteCommand:
                            idProperty = nameof(createNoteCommand.Title);
                            commandId = createNoteCommand.Title;
                            break;


                        default:
                            idProperty = "Id?";
                            commandId = "n/a";
                            break;
                    }

                    _logger.LogInformation(
                        "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        commandName,
                        idProperty,
                        commandId,
                        command);

                    // Send the embeded business command to mediator so it runs its related CommandHandler 
                    var result = await _mediator.Send(command, cancellationToken);

                    _logger.LogInformation(
                        "----- Command result: {@Result} - {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        result,
                        commandName,
                        idProperty,
                        commandId,
                        command);

                    return result;
                }
                catch
                {
                    return default(R);
                }
            }
        }
    }
}
