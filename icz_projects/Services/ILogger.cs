using System;
using Microsoft.AspNetCore.Http;

namespace icz_projects.Services
{
    public interface ILogger
    {
        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="context">HttpContext of the request</param>
        /// <param name="message">Message to write</param>
        void WriteLog(HttpContext context, string message);
    }
}
