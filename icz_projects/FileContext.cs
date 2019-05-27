using System;
namespace icz_projects
{
    public abstract class FileContext
    {
        protected bool _needReload = true;
        protected string _filePath;
        public FileContext(string filePath)
        {

        }
    }
}
