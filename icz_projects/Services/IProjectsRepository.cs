using System;
using System.Collections.Generic;
using icz_projects.Models;

namespace icz_projects.Services
{
    public interface IProjectsRepository
    {
        /// <summary>
        /// Saves project
        /// </summary>
        /// <param name="project">New Project object</param>
        void SaveProject(Project project);

        /// <summary>
        /// Gets project with specified id
        /// </summary>
        /// <returns>Project object</returns>
        /// <param name="id">Id of the project</param>
        Project GetProject(string id);

        /// <summary>
        /// Deletes the project with specified id.
        /// </summary>
        /// <param name="id">Id of the project</param>
        void DeleteProject(string id);

        /// <summary>
        /// Updates the project with specified id.
        /// </summary>
        /// <param name="id">Id of the project.</param>
        /// <param name="project">Project object with updated metadata</param>
        void UpdateProject(string id, Project project);

        /// <summary>
        /// Exists the project with specified id.
        /// </summary>
        /// <returns><c>true</c>, if project exists, <c>false</c> otherwise.</returns>
        /// <param name="id">Id of the project.</param>
        bool ExistsProject(string id);

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <returns>Array with all projects</returns>
        IEnumerable<Project> GetProjects();
    }
}
