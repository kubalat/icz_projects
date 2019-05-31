using System;
using Microsoft.AspNetCore.Http;

namespace icz_projects.Services
{
    public interface ILogger
    {
        void WriteLog(HttpContext context, string message);
    }
}
