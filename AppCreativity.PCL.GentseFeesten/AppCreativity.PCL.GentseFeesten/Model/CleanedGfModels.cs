using System.Collections.Generic;
using System.ComponentModel;

namespace AppCreativity.PCL.GentseFeesten.Model
{
    public class CleanedGFEvent
    {
        public string id { get; set; }
        public string titleId { get; set; }
        [DefaultValue("1")]
        public string gratis { get; set; }
        public object prijs { get; set; }
        public object prijs_vvk { get; set; }
        public string omschId { get; set; }
        public string datum { get; set; }
        public string periode { get; set; }
        public string start { get; set; }
        public string sort { get; set; }
        public string catId { get; set; }
        public object url { get; set; }
        public string locId { get; set; }
        //public string loc { get; set; }
        //public string lat { get; set; }
        //public string lon { get; set; }
        public object korting { get; set; }
        [DefaultValue("0")]
        public string festival { get; set; }
    }

    public class Title
    {
        public string id { get; set; }
        public string title { get; set; }
    }

    public class Description
    {
        public string id { get; set; }
        public string omsch { get; set; }
    }

    public class Location
    {
        public string id { get; set; }
        public string loc { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
    }

    public class Root
    {
        public List<CleanedGFEvent> Events { get; set; }
        public List<Title> Titles { get; set; }
        public List<Description> Descriptions { get; set; }
        public List<Location> Locations { get; set; }

        public Root()
        {
            Events = new List<CleanedGFEvent>();
            Titles = new List<Title>();
            Descriptions = new List<Description>();
            Locations = new List<Location>();
        }
    }
}
