using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using icz_projects.Datasources;
using icz_projects.Models;

namespace icz_projects.Contexts
{
    public class ProjectContext : FileContext
    {
        public IEnumerable<Project> Projects { get; set; }
        public string _timeStamp;

        public ProjectContext(string filePath) : base(filePath)
        {
            this.LoadData();
        }

        public override void LoadData()
        {
            List<Project> data = new List<Project>();
            data.Add(new Project() { Id = "Proj1", Name = "Name", Abbreviation = "Abbr", Customer = "Cust" });
            data.Add(new Project() { Id = "Proj3", Name = "Name", Abbreviation = "Abbr", Customer = "Cust" });
            data.Add(new Project() { Id = "Proj2", Name = "Name", Abbreviation = "Abbr", Customer = "Cust" });
            this.Projects = data as IEnumerable<Project>;
        }

        public override void ReloadData()
        {
            if (this._timeStamp != "")
            {
                this.LoadData();
            }
        }

        public override void SaveChanges()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Project));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(true, true);
            settings.Indent = true;
            //settings.OmitXmlDeclaration = true;  

            System.IO.FileStream file = System.IO.File.Create(this._filePath);

            serializer.Serialize(file, this.Projects);
            file.Close();
        }


        private void _deserializeObject()
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
    }
}
