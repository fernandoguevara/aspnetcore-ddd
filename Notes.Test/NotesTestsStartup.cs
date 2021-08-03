using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Notes.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Test
{
    public class NotesTestsStartup : Startup
    {
        public NotesTestsStartup(IConfiguration env) : base(env)
        {
        }

        protected override void ConfigureAuth(IApplicationBuilder app)
        {
            if (Configuration["isTest"] == bool.TrueString.ToLowerInvariant())
            {
                app.UseMiddleware<AutoAuthorizeMiddleware>();
                app.UseAuthorization();
            }
            else
            {
                base.ConfigureAuth(app);
            }
        }
    }
}
