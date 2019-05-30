using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace icz_projects.Services
{
    public interface ILoginRepository
    {
        Task<bool> Login(string password, HttpContext context);
        Task<bool> Logout(HttpContext context);
        bool IsLogged(HttpContext context);
    }
}
