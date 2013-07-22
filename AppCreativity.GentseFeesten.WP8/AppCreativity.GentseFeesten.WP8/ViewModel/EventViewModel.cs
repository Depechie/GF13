using System.Collections.ObjectModel;
using System.Device.Location;
using System.Globalization;
using AppCreativity.GentseFeesten.WP8.Framework;
using AppCreativity.GentseFeesten.WP8.Messages;
using AppCreativity.GentseFeesten.WP8.Model;
using AppCreativity.PCL.GentseFeesten.Model;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Tasks;

namespace AppCreativity.GentseFeesten.WP8.ViewModel
{
    public class EventViewModel : ViewModelBase
    {
        public string EventPropertyName = "GFEvent";
        private GFEvent _gfEvent;
        public GFEvent GFEvent
        {
            get { return _gfEvent; }
            set
            {
                if (value == _gfEvent)
                    return;

                _gfEvent = value;
                RaisePropertyChanged(EventPropertyName);
            }
        }

        public const string DetailPropertyName = "Detail";
        private ObservableCollection<DetailItem> _detail = new ObservableCollection<DetailItem>();
        public ObservableCollection<DetailItem> Detail
        {
            get { return _detail; }
            set
            {
                if (value == _detail)
                    return;

                _detail = value;
                RaisePropertyChanged(DetailPropertyName);
            }
        }

        public GeoCoordinate MapCenter
        {
            get
            {
                return new GeoCoordinate(double.Parse(this.GFEvent.lat, new CultureInfo("en-US")), double.Parse(this.GFEvent.lon, new CultureInfo("en-US")));
            }
        }

        public RelayCommand RouteCommand { get; private set; }
        public RelayCommand WebCommand { get; private set; }

        public EventViewModel(INavigationService navigationService, IMessenger messenger) : base(messenger)
        {
            MessengerInstance.Register<SelectedEventMessage>(this, message =>
                {
                    this.GFEvent = message.Event;
                    this.Detail.Clear();
                    this.Detail.Add(new DetailItem()
                        {
                            Index = 1,
                            Label = "Datum:",
                            Name = string.Format("{0}\n{1}", message.SelectedDate, this.GFEvent.periode),
                            BorderThickness = "0,0,0,1",
                            BorderMargin = "12,6,12,0"
                        });
                    this.Detail.Add(new DetailItem()
                        {
                            Index = 2,
                            Label = "Prijs:",
                            Name = this.GFEvent.prijs ?? "Gratis",
                            BorderThickness = "0,0,0,1",
                            BorderMargin = "12,0,12,0"
                        });
                    this.Detail.Add(new DetailItem()
                        {
                            Index = 3,
                            Label = "Locatie:",
                            Name = this.GFEvent.loc,
                            BorderThickness = !string.IsNullOrEmpty(this.GFEvent.omsch) ? "0,0,0,1" : "0,0,0,0",
                            BorderMargin = !string.IsNullOrEmpty(this.GFEvent.omsch) ? "12,0,12,0" : "12,0,12,6"
                        });
                    if (!string.IsNullOrEmpty(this.GFEvent.omsch))
                    {
                        this.Detail.Add(new DetailItem()
                        {
                            Index = 4,
                            Label = "",
                            Name = this.GFEvent.omsch,
                            BorderThickness = "0,0,0,0",
                            BorderMargin = "12,0,12,6"
                        });
                    }
                });

            this.RouteCommand = new RelayCommand(() =>
            {
                GeoCoordinate toPoint = this.MapCenter;
                LabeledMapLocation toPointLML = new LabeledMapLocation(this.GFEvent.title, toPoint);

                BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();
                bingMapsDirectionsTask.End = toPointLML;
                bingMapsDirectionsTask.Show();
            });
            this.WebCommand = new RelayCommand(() => Helpers.NavigateToWebsite(this.GFEvent.weblink));
        }
    }
}
