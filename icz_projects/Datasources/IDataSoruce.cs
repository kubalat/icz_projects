using System;
namespace icz_projects.Datasources
{
    public interface IDataSoruce
    {
        /// <summary>
        /// Save changes from arrays to source
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Loads data from source to arrays
        /// </summary>
        void LoadData();
    }
}
