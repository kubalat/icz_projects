using System;
namespace icz_projects.Datasources
{
    public interface IDataSoruce
    {
        void SaveChanges();
        void LoadData();
    }
}
