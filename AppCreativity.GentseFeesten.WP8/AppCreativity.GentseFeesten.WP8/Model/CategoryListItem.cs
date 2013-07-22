using AppCreativity.PCL.GentseFeesten.Model;

namespace AppCreativity.GentseFeesten.WP8.Model
{
    public class CategoryListItem
    {
        public GFCategory Category { get; set; }
        public string BorderThickness { get; set; }
        public string BorderMargin { get; set; }

        public CategoryListItem(GFCategory category, string borderThickness, string borderMargin)
        {
            this.Category = category;
            this.BorderThickness = borderThickness;
            this.BorderMargin = borderMargin;
        }
    }
}
