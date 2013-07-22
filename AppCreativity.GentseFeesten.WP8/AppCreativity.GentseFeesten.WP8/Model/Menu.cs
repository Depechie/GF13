namespace AppCreativity.GentseFeesten.WP8.Model
{
    public class Menu
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string BorderThickness { get; set; }
        public string BorderMargin { get; set; }
    }

    public class DetailItem : Menu
    {
        public string Label { get; set; }
    }
}
