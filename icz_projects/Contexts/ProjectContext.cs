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

        /// <summary>
        /// Initializes a new instance of the this class.
        /// </summary>
        /// <param name="filePath">File path to datasource</param>
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

        /// <summary>
        /// Loads the data from file to the array.
        /// </summary>
        public override void LoadData()
        {
            try
            {
                //Create directory
                Directory.CreateDirectory(Path.GetDirectoryName(this._filePath));

                if (File.Exists(this._filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Project>), new XmlRootAttribute("Projects"));

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Encoding = Encoding.GetEncoding(1250);
                    settings.Indent = true;

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

        /// <summary>
        /// Saves the changes from array to file.
        /// </summary>
        public override void SaveChanges()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Project>), new XmlRootAttribute("Projects"));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.GetEncoding(1250);
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
