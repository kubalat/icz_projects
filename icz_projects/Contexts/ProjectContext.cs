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
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "Parameter was null");
            }
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public override void LoadData()
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public override void SaveChanges()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Project>));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(true, true);
                settings.Indent = true;

                using (FileStream file = File.Create(this._filePath))
                {
                    serializer.Serialize(file, this.Projects as List<Project>);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
