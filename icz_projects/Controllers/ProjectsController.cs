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
        private readonly ILogger _logger;

        public ProjectsController(IProjectsRepository repository, ILogger logger)
        {
            this._repository = repository;
            this._logger = logger;

        }

        public IActionResult Index()
        {

            string msgToShow = TempData["ErrorMessage"] as string;
            if (!string.IsNullOrWhiteSpace(msgToShow))
            {
                ViewBag.Msg = msgToShow;
            }

            try
            {
                this._logger.WriteLog(HttpContext, "Projects - Index - Get projects");
                ViewBag.Projects = this._repository.GetProjects();
                return View();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Parameter id is null");
            }

            try
            {
                this._logger.WriteLog(HttpContext, "Projects - Details - Get project: " + id);
                Project p = this._repository.GetProject(id);

                if (p == null)
                {
                    return NotFound("Project not found.");
                }
                else
                {
                    return View(p);
                }
            }
            catch (Exception ex)
            {
                //TODO
            }
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            this._repository.DeleteProject(id);
            TempData["ErrorMessage"] = "Project was deleted.";
            this._logger.WriteLog(HttpContext, "Projects - Delete - Delete project: " + id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult New()
        {
            this._logger.WriteLog(HttpContext, "Projects - New - New project");
            return View(new Project());
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            this._logger.WriteLog(HttpContext, "Projects - Edit - Edit project: " + id);
            return View(this._repository.GetProject(id));
        }


        [HttpPost]
        public IActionResult Create(Project project)
        {
            this._repository.SaveProject(project);
            TempData["ErrorMessage"] = "Project was created.";
            this._logger.WriteLog(HttpContext, "Projects - Create - Create project");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(string id, Project project)
        {
            this._repository.UpdateProject(id, project);
            TempData["ErrorMessage"] = "Project was updated.";
            this._logger.WriteLog(HttpContext, "Projects - Update - Update project: " + id);
            return RedirectToAction("Index");
        }
    }
}
