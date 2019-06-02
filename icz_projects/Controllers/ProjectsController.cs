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

        /// <summary>
        /// Initializes a new instance of the this class.
        /// </summary>
        /// <param name="repository">IProjectsRepository for projects</param>
        /// <param name="logger">ILogger for writing logs</param>
        public ProjectsController(IProjectsRepository repository, ILogger logger)
        {
            this._repository = repository;
            this._logger = logger;

        }

        /// <summary>
        /// Page with all projects
        /// </summary>
        public IActionResult Index()
        {
            try
            { 
                string msgToShow = TempData["ErrorMessage"] as string;
                if (!string.IsNullOrWhiteSpace(msgToShow))
                {
                    ViewBag.Msg = msgToShow;
                }


                this._logger.WriteLog(HttpContext, "Projects - Index - Get projects");
                ViewBag.Projects = this._repository.GetProjects();
                return View();
            }
            catch (Exception ex)
            {
                this._logger.WriteLog(HttpContext, "Projects - Index - Error: " + ex.Message + " - " + ex.ToString());
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Page with details of project with the specified id.
        /// </summary>
        /// <param name="id">Id of the project</param>
        [HttpGet]
        public IActionResult Details(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    this._logger.WriteLog(HttpContext, "Projects - Details - BadRequest: Parameter id is null");
                    return BadRequest("Parameter id is null");
                }

                if (!this._repository.ExistsProject(id))
                {
                    this._logger.WriteLog(HttpContext, "Projects - Details - NotFound: Project not exists");
                    return NotFound("Project not exists.");
                }

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
                this._logger.WriteLog(HttpContext, "Projects - Details - Error: " + ex.Message + " - " + ex.ToString());
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Delete project with the specified id.
        /// </summary>
        /// <param name="id">Id of the project</param>
        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    this._logger.WriteLog(HttpContext, "Projects - Delete - BadRequest: Parameter id is null");
                    return BadRequest("Parameter id is null");
                }

                if (!this._repository.ExistsProject(id))
                {
                    this._logger.WriteLog(HttpContext, "Projects - Delete - NotFound: Project not exists");
                    return NotFound("Project not exists.");
                }


                this._repository.DeleteProject(id);
                TempData["ErrorMessage"] = "Project was deleted.";
                this._logger.WriteLog(HttpContext, "Projects - Delete - Delete project: " + id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this._logger.WriteLog(HttpContext, "Projects - Delete - Error: " + ex.Message + " - " + ex.ToString());
                return StatusCode(500, ex.Message);
            }

        }

        /// <summary>
        /// New form page for project
        /// </summary>
        [HttpGet]
        public IActionResult New()
        {
            try
            {
                this._logger.WriteLog(HttpContext, "Projects - New - New project");
                return View(new Project());
            }
            catch (Exception ex)
            {
                this._logger.WriteLog(HttpContext, "Projects - New - Error: " + ex.Message + " - " + ex.ToString());
                return StatusCode(500, ex.Message);

            }


        }

        /// <summary>
        /// Edit page for the project with the specified id.
        /// </summary>
        /// <returns>The edit.</returns>
        /// <param name="id">Id of the project</param>
        [HttpGet]
        public IActionResult Edit(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    this._logger.WriteLog(HttpContext, "Projects - Edit - BadRequest: Parameter id is null");
                    return BadRequest("Parameter id is null");
                }

                if (!this._repository.ExistsProject(id))
                {
                    this._logger.WriteLog(HttpContext, "Projects - Edit - NotFound: Project not exists");
                    return NotFound("Project not exists.");
                }


                this._logger.WriteLog(HttpContext, "Projects - Edit - Edit project: " + id);
                return View(this._repository.GetProject(id));
            }
            catch (Exception ex)
            {
                this._logger.WriteLog(HttpContext, "Projects - Edit - Error: " + ex.Message + " - " + ex.ToString());
                return StatusCode(500, ex.Message);
            }

        }

        /// <summary>
        /// Create project
        /// </summary>
        /// <param name="project">New Project object</param>
        [HttpPost]
        public IActionResult Create(Project project)
        {
            try
            {
                if (project == null)
                {
                    this._logger.WriteLog(HttpContext, "Projects - Create - BadRequest: Parameter project is null");
                    return BadRequest("Parameter project is null");
                }


                this._repository.SaveProject(project);
                TempData["ErrorMessage"] = "Project was created.";
                this._logger.WriteLog(HttpContext, "Projects - Create - Create project");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this._logger.WriteLog(HttpContext, "Projects - Create - Error: " + ex.Message + " - " + ex.ToString());
                return StatusCode(500, ex.Message);
            }



        }

        /// <summary>
        /// Update project with the specified id
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="id">Id of the project</param>
        /// <param name="project">Project object with updated metadata</param>
        [HttpPost]
        public IActionResult Update(string id, Project project)
        {
            try
            {
                if (project == null)
                {
                    this._logger.WriteLog(HttpContext, "Projects - Update - BadRequest: Parameter project is null");
                    return BadRequest("Parameter project is null");
                }

                if (string.IsNullOrWhiteSpace(id))
                {
                    this._logger.WriteLog(HttpContext, "Projects - Update - BadRequest: Parameter id is null");
                    return BadRequest("Parameter id is null");
                }

                if (!this._repository.ExistsProject(id))
                {
                    this._logger.WriteLog(HttpContext, "Projects - Update - NotFound: Project not exists");
                    return NotFound("Project not exists.");
                }


                this._repository.UpdateProject(id, project);
                TempData["ErrorMessage"] = "Project was updated.";
                this._logger.WriteLog(HttpContext, "Projects - Update - Update project: " + id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this._logger.WriteLog(HttpContext, "Projects - Update - Error: " + ex.Message + " - " + ex.ToString());
                return StatusCode(500, ex.Message);
            }

        }
    }
}
