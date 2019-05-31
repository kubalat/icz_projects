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
            List<string> projectsIdsString = this._context.Projects.Select(pr => pr.Id).ToList();
            List<int> projectIds = new List<int>();

            foreach (string idString in projectsIdsString)
            {
                projectIds.Add(Convert.ToInt32(idString.Replace("proj", "")));
            }

            IEnumerable<int> temp = projectIds as IEnumerable<int>;
            int nextId = temp.Max() + 1;
            project.Id = "proj" + nextId.ToString();

            List<Project> proj = this._context.Projects as List<Project>;
            proj.Add(project);
            _context.SaveChanges();
        }

        public void UpdateProject(string id, Project project)
        {
            IEnumerable<Project> foundedProject = this._context.Projects.Where(p => p.Id == id) as IEnumerable<Project>;
            if (foundedProject.Any())
            {
                Project p = foundedProject.First();

                p.Abbreviation = project.Abbreviation;
                p.Customer = project.Customer;
                p.Name = project.Name;

                _context.SaveChanges();
            }
            else
            {
                //TODO not found

            }
        }
    }
}
