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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Parameter is null");
            }
            this._context = context;
        }

        public void DeleteProject(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Parameter is null");
            }

            try
            {
                IEnumerable<Project> foundedProject = this._context.Projects.Where(project => project.Id == id);
                if (foundedProject.Any())
                {
                    List<Project> tempList = this._context.Projects as List<Project>;
                    tempList.Remove(foundedProject.First());
                    _context.SaveChanges();
                }
                else
                {
                    throw new ArgumentNullException(nameof(Project), "Project not found.");
                }
            }
            catch (Exception ex)
            {

            }

        }

        public Project GetProject(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Parameter is null");
            }
            try
            {
                IEnumerable<Project> foundedProject = this._context.Projects.Where(project => project.Id.ToLower() == id.ToLower()) as IEnumerable<Project>;
                if (foundedProject.Any())
                {
                    return foundedProject.First();
                }
                else
                {
                    //throw new ArgumentNullException(nameof(Project), "Project not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public IEnumerable<Project> GetProjects()
        {
            IEnumerable<Project> projects = this._context.Projects;
            if (projects == null)
            {
                throw new ArgumentNullException(nameof(projects), "Parameter is null");
            }
            return projects;
        }

        public void SaveProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Parameter is null");
            }

            try
            {
                //Get all ids and generate id for new project
                List<string> projectsIdsString = this._context.Projects.Select(pr => pr.Id).ToList();
                int nextId = 1;

                if (projectsIdsString.Any())
                {
                    List<int> projectIds = new List<int>();

                    foreach (string idString in projectsIdsString)
                    {
                        projectIds.Add(Convert.ToInt32(idString.Replace("proj", "")));
                    }

                    IEnumerable<int> temp = projectIds as IEnumerable<int>;
                    nextId = temp.Max() + 1;
                }




                project.Id = "proj" + nextId.ToString();

                List<Project> proj = this._context.Projects as List<Project>;
                proj.Add(project);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public void UpdateProject(string id, Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Parameter is null");
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Parameter is null");
            }

            try
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
                    throw new ArgumentNullException(nameof(Project), "Project not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
