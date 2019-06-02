using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace icz_projects.Services
{
    public interface ILoginRepository
    {
        /// <summary>
        /// Login to the system.
        /// </summary>
        /// <returns><c>true</c>, if login was successful, <c>false</c> otherwise.</returns>
        /// <param name="password">Password for login to the system</param>
        /// <param name="context">HttpContext of the request</param>
        Task<bool> Login(string password, HttpContext context);

        /// <summary>
        /// Logout from the system
        /// </summary>
        /// <returns><c>true</c>, if logout was successful, <c>false</c> otherwise.</returns>
        /// <param name="context">HttpContext of the request</param>
        Task<bool> Logout(HttpContext context);

        /// <summary>
        /// Check if already logged in system.
        /// </summary>
        /// <returns><c>true</c>, if logged, <c>false</c> otherwise.</returns>
        /// <param name="context">HttpContext of the request</param>
        bool IsLogged(HttpContext context);
    }
}
