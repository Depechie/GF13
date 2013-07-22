using AppCreativity.PCL.GentseFeesten.Model;
using GalaSoft.MvvmLight.Messaging;

namespace AppCreativity.GentseFeesten.WP8.Messages
{
    public class SelectedEventMessage : MessageBase
    {
        public GFEvent Event { get; private set; }
        public string SelectedDate { get; private set; }

        public SelectedEventMessage(GFEvent gfEvent, string selectedDate)
        {
            this.Event = gfEvent;
            this.SelectedDate = selectedDate;
        }
    }
}
