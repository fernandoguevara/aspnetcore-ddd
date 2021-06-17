﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.API.Application.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string message);
    }
}
