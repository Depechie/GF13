using System;

namespace AppCreativity.PCL.GentseFeesten.Model
{
    public class GFEvent
    {
        public string title { get; set; }
        public string id { get; set; }
        public string gratis { get; set; }
        public string prijs { get; set; }
        public string prijs_vvk { get; set; }
        public string omsch { get; set; }
        public string datum { get; set; }
        public string periode { get; set; }
        public string start { get; set; }
        public string sort { get; set; }
        public string cat { get; set; }
        public string cat_id { get; set; }
        public string url { get; set; }
        public string loc_id { get; set; }
        public string loc { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string korting { get; set; }
        public string festival { get; set; }
        public string weblink
        {
            get { return string.Format("http://gentsefeesten.be/event/{0}", id); }
        }
    }

    public class GFCategory
    {
        public string id { get; set; }
        public string title { get; set; }

        public override string ToString()
        {
            return title;
        }
    }

    public class GFDay
    {
        public int timestamp { get; set; }
        public string day { get; set; }
    }
}
