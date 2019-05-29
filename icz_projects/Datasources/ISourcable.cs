using System;
namespace icz_projects.Datasources
{
    public interface ISourcable
    {
        void SaveChanges();
        void LoadData();
        void ReloadData();
    }
}
