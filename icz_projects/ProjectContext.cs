using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using icz_projects.Models;
using Microsoft.EntityFrameworkCore;

namespace icz_projects
{
    public class ProjectContext : FileContext, IDataSource
    {
        public IEnumerable<Project> Projects { get; set; }

        public ProjectContext(string filePath) : base(filePath)
        {
            this._filePath = filePath;
            this.LoadData();
        }

        public void LoadData()
        {
            this.DeserializeObject();
        }

        private void DeserializeObject()
        {
            
            XmlSerializer serializer = new XmlSerializer(typeof(List<Project>));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(true, true);
            settings.Indent = true;
            //settings.OmitXmlDeclaration = true;  

            using (Stream reader = new FileStream(this._filePath, FileMode.Open))
            {
                this.Projects = serializer.Deserialize(reader) as IEnumerable<Project>;
            }
        }

        //Override default saving changes
        public void SaveChanges()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(this._filePath);
            this.ReloadData();

            /*XmlSerializer ser = new XmlSerializer();
            FileStream myFileStream = new FileStream(_env.ContentRootPath + "\\Countries\\en-GB.xml", FileMode.Open);
            return ((Container)ser.Deserialize(myFileStream)).Countries;*/
        }

        private void SerializeObject<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(true, true);
            settings.Indent = true;
            //settings.OmitXmlDeclaration = true;  

            System.IO.FileStream file = System.IO.File.Create(this._filePath);

            serializer.Serialize(file, obj);
            file.Close();
        }

        public void ReloadData()
        {
            //throw new NotImplementedException();
        }

        public object GetCollection(string name)
        {
            return typeof(ProjectContext).GetProperty(name).GetValue(this, null);
        }

        public void SetCollection(string name, object value)
        {
            typeof(ProjectContext).GetProperty(name).SetValue(this, value);
        }
    }
}
