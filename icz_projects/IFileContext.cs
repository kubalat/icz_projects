using System;
using System.Collections.Generic;

namespace icz_projects
{
    public interface IDataSource
    {
        object GetCollection(string name);
        void SetCollection(string name, object value);
        void SaveChanges();
        void ReloadData();
    }
}
