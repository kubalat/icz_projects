using System;
using System.Collections.Generic;
using System.Linq;
using icz_projects.Models;

namespace icz_projects.Services
{
    public class ProjectsRepository : IProjectsRepository
    {
        private IDataSource _context;
        IEnumerable<Project> _projects;

        public ProjectsRepository(IDataSource context)
        {
            this._context = context;
            this._projects = this._context.GetCollection("Projects") as IEnumerable<Project>;
        }

        public void DeleteProject(string id)
        {
            IEnumerable<Project> foundedProject = this._projects.Select(project => project.Id == id) as IEnumerable<Project>;
            if (foundedProject.Any())
            {
                _context.SetCollection("Projects", this._projects.Select(project => project.Id != id));
                _context.SaveChanges();
            }
            else
            {
                //TODO not found
            }
        }

        public Project GetProject(string id)
        {
            IEnumerable<Project> foundedProject = this._projects.Select(project => project.Id == id) as IEnumerable<Project>;
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
            return this._projects;
        }

        public void SaveProject(Project project)
        {
            this._projects.Append(project);
            _context.SaveChanges();
        }

        public void UpdateProject(Project project)
        {
            IEnumerable<Project> foundedProject = this._projects.Select(p => p.Id == project.Id) as IEnumerable<Project>;
            if (foundedProject.Any())
            {
                Project p = foundedProject.First();

                //TODO Is this working?
                p = project;

                _context.SaveChanges();
            }
            else
            {
                //TODO not found

            }
        }
    }
}
