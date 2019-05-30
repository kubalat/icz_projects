using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace icz_projects.Models
{
    [XmlRoot("Projects")]
    public class Project
    {
        [DisplayName("ID")]
        [XmlAttributeAttribute("Id")]
        public string Id { get; set; }

        [DisplayName("Name")]
        [XmlElement("Name")]
        public string Name { get; set; }

        [DisplayName("Abbreviation")]
        [XmlElement("Abbreviation")]
        public string Abbreviation { get; set; }

        [DisplayName("Customer")]
        [XmlElement("Customer")]
        public string Customer { get; set; }
    }
}
