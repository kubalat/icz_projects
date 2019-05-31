using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace icz_projects.Services
{
    public class Logger : ILogger
    {
        private readonly string _filePath;
        public Logger(string filePath)
        {
            this._filePath = filePath;
        }

        public void WriteLog(HttpContext context, string message)
        {
            using (StreamWriter w = File.AppendText(this._filePath))
            {
                w.WriteLine(String.Format("{0} - {1} - {2}", DateTime.Now.ToString(), context.Connection.RemoteIpAddress.ToString(), message));

            }
        }
    }
}
