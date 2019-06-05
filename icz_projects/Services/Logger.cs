using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace icz_projects.Services
{
    public class Logger : ILogger
    {
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="filePath">File path to the log file</param>
        public Logger(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "Parameter is null");
            }
            this._filePath = filePath;
        }

        /// <summary>
        /// Writes the log to the file.
        /// </summary>
        /// <param name="context">HttpContext of request</param>
        /// <param name="message">Message to log</param>
        public void WriteLog(HttpContext context, string message)
        {
            try
            {
                //Create directory
                Directory.CreateDirectory(Path.GetDirectoryName(this._filePath));

                //Write log
                using (StreamWriter w = File.AppendText(this._filePath))
                {
                    w.WriteLine(String.Format("{0} - {1} - {2}", DateTime.Now.ToString(), context.Connection.RemoteIpAddress.ToString(), message));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
    }
}
