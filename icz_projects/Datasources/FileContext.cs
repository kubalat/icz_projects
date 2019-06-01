using System;
namespace icz_projects.Datasources
{
    public abstract class FileContext : IDataSoruce
    {
        protected string _filePath;
        public FileContext(string filePath)
        {
            this._filePath = filePath;
        }

        public abstract void LoadData();
        public abstract void SaveChanges();
    }
}
