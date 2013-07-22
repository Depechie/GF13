using AppCreativity.PCL.GentseFeesten.Model;

namespace AppCreativity.GentseFeesten.WP8.Model
{
    public class DayListItem
    {
        public GFDay Day { get; set; }
        public string BorderThickness { get; set; }
        public string BorderMargin { get; set; }

        public DayListItem(GFDay day, string borderThickness, string borderMargin)
        {
            this.Day = day;
            this.BorderThickness = borderThickness;
            this.BorderMargin = borderMargin;
        }
    }
}
