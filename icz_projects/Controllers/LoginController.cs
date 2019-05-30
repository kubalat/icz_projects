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
        public LoginController(ILoginRepository repository)
        {
            this._repository = repository;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            if (this._repository.IsLogged(HttpContext))
            {
                TempData["ErrorMessage"] = "You are already logged in.";
                return RedirectToAction("Index", "Projects");
            }
            else
            {
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
                TempData["ErrorMessage"] = "You are already logged in.";
                return RedirectToAction("Index", "Projects");
            }
                


            if (await this._repository.Login(password, HttpContext))
            {
                TempData["ErrorMessage"] = "Login successful.";
                return RedirectToAction("Index", "Projects");
            }
            else
            {
                TempData["ErrorMessage"] = "Wrong password.";
                return RedirectToAction("Index", "Login");
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (await this._repository.Logout(HttpContext))
            {
                TempData["ErrorMessage"] = "Logout successful.";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrorMessage"] = "Error while logouting.";
                return RedirectToAction("Index", "Login");
            }
        }
    }



}
