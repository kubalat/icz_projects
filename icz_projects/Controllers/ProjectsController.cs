using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using icz_projects.Models;
using icz_projects.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace icz_projects.Controllers
{
    public class ProjectsController : Controller
    { 
        private readonly IProjectsRepository _repository;

        public ProjectsController(IProjectsRepository repository)
        {
            this._repository = repository;
        }

        public IActionResult Index()
        {
            ViewBag.Projects = this._repository.GetProjects();
            return View();
        }

        public IActionResult Details(int id)
        { 
            ViewBag.Project = this._repository.GetProject(id.ToString());
            return View();
        }
    }
}
