using System;
using System.Collections.Generic;
using icz_projects.Models;

namespace icz_projects.Services
{
    public interface IProjectsRepository
    {
        void SaveProject(Project project);
        IEnumerable<Project> GetProjects();
        Project GetProject(string id);
        void DeleteProject(string id);
        void UpdateProject(Project project);
    }
}
