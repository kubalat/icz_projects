using System;
using System.Collections.Generic;
using System.Linq;
using icz_projects.Contexts;
using icz_projects.Models;

namespace icz_projects.Services
{
    public class ProjectsRepository : IProjectsRepository
    {
        private ProjectContext _context;

        public ProjectsRepository(ProjectContext context)
        {
            this._context = context;

        }

        public void DeleteProject(string id)
        {
            IEnumerable<Project> foundedProject = this._context.Projects.Where(project => project.Id == id);
            if (foundedProject.Any())
            {
                List<Project> tempList = this._context.Projects as List<Project>;
                tempList.Remove(foundedProject.First());
                //_context.SaveChanges();
            }
            else
            {
                //TODO not found
            }
        }

        public Project GetProject(string id)
        {
            IEnumerable<Project> foundedProject = this._context.Projects.Where(project => project.Id.ToLower() == id.ToLower()) as IEnumerable<Project>;
            if (foundedProject.Any())
            {
                return foundedProject.First();
            }
            else
            {
                //TODO not found
                return null;
            }
        }

        public IEnumerable<Project> GetProjects()
        {
            return this._context.Projects;
        }

        public void SaveProject(Project project)
        {
            this._context.Projects.Append(project);
            _context.SaveChanges();
        }

        public void UpdateProject(Project project)
        {
            IEnumerable<Project> foundedProject = this._context.Projects.Where(p => p.Id == project.Id) as IEnumerable<Project>;
            if (foundedProject.Any())
            {
                Project p = foundedProject.First();

                List<Project> tempList = this._context.Projects as List<Project>;
                tempList[tempList.IndexOf(p)] = project;

                //_context.SaveChanges();
            }
            else
            {
                //TODO not found

            }
        }
    }
}
