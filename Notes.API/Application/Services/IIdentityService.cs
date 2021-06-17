using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.API.Application.Services
{
    public interface IIdentityService
    {
        string GetUserIdentity();
        string GetUserName();
        string GetUserEmail();
    }
}
