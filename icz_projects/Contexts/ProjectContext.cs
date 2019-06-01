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
        [XmlElement("Projects")]
        public IEnumerable<Project> Projects { get; set; }
        public string _timeStamp;

        public ProjectContext(string filePath) : base(filePath)
        {
            this.LoadData();
        }

        public override void LoadData()
        {
            if (File.Exists(this._filePath))
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
            else
            {
                List<Project> data = new List<Project>();
                this.Projects = data as IEnumerable<Project>;
            }
        }

        public override void SaveChanges()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Project>));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(true, true);
            settings.Indent = true;
            //settings.OmitXmlDeclaration = true;  

            System.IO.FileStream file = System.IO.File.Create(this._filePath);

            serializer.Serialize(file, this.Projects as List<Project>);
            file.Close();
        }
    }
}
