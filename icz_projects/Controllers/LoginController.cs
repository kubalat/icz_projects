using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using icz_projects.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace icz_projects.Controllers
{
    public class LoginController : Controller
    {
        private ILoginRepository _repository;
        private ILogger _logger;
        public LoginController(ILoginRepository repository, ILogger logger)
        {
            this._repository = repository;
            this._logger = logger;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            if (this._repository.IsLogged(HttpContext))
            {
                this._logger.WriteLog(HttpContext, "Login - Index - User is already logged. Redirecting.");
                TempData["ErrorMessage"] = "You are already logged in.";
                return RedirectToAction("Index", "Projects");
            }
            else
            {
                this._logger.WriteLog(HttpContext, "Login - Index - Login form");
                string msgToShow = TempData["ErrorMessage"] as string;
                if (!string.IsNullOrWhiteSpace(msgToShow))
                {
                    ViewBag.Msg = msgToShow;
                }
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(string password)
        {
            if (this._repository.IsLogged(HttpContext))
            {
                this._logger.WriteLog(HttpContext, "Login - Index - User is already logged. Redirecting.");
                TempData["ErrorMessage"] = "You are already logged in.";
                return RedirectToAction("Index", "Projects");
            }
                


            if (await this._repository.Login(password, HttpContext))
            {
                this._logger.WriteLog(HttpContext, "Login - Login - Login succesful.");
                TempData["ErrorMessage"] = "Login successful.";
                return RedirectToAction("Index", "Projects");
            }
            else
            {
                this._logger.WriteLog(HttpContext, "Login - Login - Wrong password.");
                TempData["ErrorMessage"] = "Wrong password.";
                return RedirectToAction("Index", "Login");
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (await this._repository.Logout(HttpContext))
            {
                this._logger.WriteLog(HttpContext, "Login - Logout - Logout successful.");
                TempData["ErrorMessage"] = "Logout successful.";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                this._logger.WriteLog(HttpContext, "Login - Logout - Error while logouting.");
                TempData["ErrorMessage"] = "Error while logouting.";
                return RedirectToAction("Index", "Login");
            }
        }
    }



}
