using GalaSoft.MvvmLight.Messaging;

namespace AppCreativity.GentseFeesten.WP8.Messages
{
    public class PivotChangeMessage : MessageBase
    {
        public string PivotName { get; set; }
        public bool Visible { get; set; }

        public PivotChangeMessage(string pivotName, bool visible)
        {
            this.PivotName = pivotName;
            this.Visible = visible;
        }
    }
}
