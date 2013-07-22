using AppCreativity.PCL.GentseFeesten.Model;

namespace AppCreativity.GentseFeesten.WP8.Model
{
    public class EventListItem
    {
        public GFEvent GFEvent { get; set; }
        public string BorderThickness { get; set; }
        public string BorderMargin { get; set; }

        public EventListItem(GFEvent gfEvent, string borderThickness, string borderMargin)
        {
            this.GFEvent = gfEvent;
            this.BorderThickness = borderThickness;
            this.BorderMargin = borderMargin;
        }
    }
}
