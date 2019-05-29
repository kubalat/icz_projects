using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace icz_projects.Models
{
    [XmlRoot("Projects")]
    public class Project
    {
        [XmlAttributeAttribute("Id")]
        public string Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Abbreviation")]
        public string Abbreviation { get; set; }

        [XmlElement("Customer")]
        public string Customer { get; set; }
    }
}
