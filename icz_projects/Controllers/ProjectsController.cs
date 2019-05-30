using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using icz_projects.Models;
using icz_projects.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace icz_projects.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    { 
        private readonly IProjectsRepository _repository;

        public ProjectsController(IProjectsRepository repository)
        {
            this._repository = repository;
        }

        public IActionResult Index()
        {
            string msgToShow = TempData["ErrorMessage"] as string;
            if (!string.IsNullOrWhiteSpace(msgToShow))
            {
                ViewBag.Msg = msgToShow;
            }

            ViewBag.Projects = this._repository.GetProjects();
            return View();
        }

        [HttpGet]
        public IActionResult Details(string id)
        { 
            return View(this._repository.GetProject(id));
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            this._repository.DeleteProject(id);
            ViewBag.Projects = this._repository.GetProjects();
            TempData["ErrorMessage"] = "Project was deleted.";
            return RedirectToAction("Index");
        }
    }
}
