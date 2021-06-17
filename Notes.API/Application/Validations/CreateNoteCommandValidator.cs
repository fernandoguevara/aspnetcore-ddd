using FluentValidation;
using Microsoft.Extensions.Logging;
using Notes.API.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.API.Application.Validations
{
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator(ILogger<CreateNoteCommand> logger)
        {
            RuleFor(command => command.Title).NotEmpty().Length(5, 50);
            RuleFor(command => command.Description).NotEmpty().Length(5, 200);
            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);

        }
    }
}
