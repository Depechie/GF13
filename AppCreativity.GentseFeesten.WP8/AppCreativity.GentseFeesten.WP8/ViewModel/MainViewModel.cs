using System.Threading;
using AppCreativity.GentseFeesten.WP8.Messages;
using AppCreativity.GentseFeesten.WP8.Model;
using AppCreativity.PCL.GentseFeesten.Framework;
using AppCreativity.PCL.GentseFeesten.Model;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace AppCreativity.GentseFeesten.WP8.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly string _installFolder = Package.Current.InstalledLocation.Path;
        private string _selectedDayTimestamp;
        private string _selectedDay;
        private List<GFEvent> _allGFEvents = new List<GFEvent>();

        public const string CategoryPivotVisibilityPropertyName = "CategoryPivotVisibility";
        private bool _categoryPivotVisibility = false;
        public bool CategoryPivotVisibility
        {
            get { return _categoryPivotVisibility; }
            set
            {
                if (value == _categoryPivotVisibility)
                    return;                

                _categoryPivotVisibility = value;
                RaisePropertyChanged(CategoryPivotVisibilityPropertyName);
            }
        }

        public const string DayPivotVisibilityPropertyName = "DayPivotVisibility";
        private bool _dayPivotVisibility = false;
        public bool DayPivotVisibility
        {
            get { return _dayPivotVisibility; }
            set
            {
                if (value == _dayPivotVisibility)
                    return;

                _dayPivotVisibility = value;
                RaisePropertyChanged(DayPivotVisibilityPropertyName);
            }
        }

        public const string EventPivotVisibilityPropertyName = "EventPivotVisibility";
        private bool _eventPivotVisibility = false;
        public bool EventPivotVisibility
        {
            get { return _eventPivotVisibility; }
            set
            {
                if (value == _eventPivotVisibility)
                    return;

                _eventPivotVisibility = value;
                RaisePropertyChanged(EventPivotVisibilityPropertyName);
            }
        }

        public const string SelectedCategoryPropertyName = "SelectedCategory";
        private GFCategory _selectedCategory;
        public GFCategory SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (value == _selectedCategory)
                    return;

                _selectedCategory = value;
                RaisePropertyChanged(SelectedCategoryPropertyName);
            }
        }

        public const string MenuPropertyName = "Menu";
        private ObservableCollection<Menu> _menu = new ObservableCollection<Menu>();
        public ObservableCollection<Menu> Menu
        {
            get { return _menu; }
            set
            {
                if (value == _menu)
                    return;

                _menu = value;
                RaisePropertyChanged(MenuPropertyName);
            }
        }

        public const string CategoriesPropertyName = "Categories";
        private ObservableCollection<CategoryListItem> _categories = new ObservableCollection<CategoryListItem>();
        public ObservableCollection<CategoryListItem> Categories
        {
            get { return _categories; }
            set
            {
                if (value == _categories)
                    return;

                _categories = value;
                RaisePropertyChanged(CategoriesPropertyName);
            }
        }

        public const string DaysPropertyName = "Days";
        private ObservableCollection<DayListItem> _days = new ObservableCollection<DayListItem>();
        public ObservableCollection<DayListItem> Days
        {
            get { return _days; }
            set
            {
                if (value == _days)
                    return;

                _days = value;
                RaisePropertyChanged(DaysPropertyName);
            }
        }

        public const string EventsPropertyName = "Events";
        private ObservableCollection<EventListItem> _events = new ObservableCollection<EventListItem>();
        public ObservableCollection<EventListItem> Events
        {
            get { return _events; }
            set
            {
                if (value == _events)
                    return;

                _events = value;
                RaisePropertyChanged(EventsPropertyName);
            }
        }

        public RelayCommand<string> MenuCommand { get; private set; }
        public RelayCommand<string> CategoryCommand { get; private set; }
        public RelayCommand<string> DayCommand { get; private set; }
        public RelayCommand<string> EventCommand { get; private set; }
        public RelayCommand AboutCommand { get; private set; }

        public MainViewModel(INavigationService navigationService, IMessenger messenger) : base(messenger)
        {
            _navigationService = navigationService;
            this.Menu.Add(new Menu()
            {
                Index = 1,
                Name = "Categorieën",
                BorderThickness = "0,0,0,0",
                BorderMargin = "12,6,12,0"
            });


            //this.Menu.Add(new Menu()
            //    {
            //        Index = 1,
            //        Name = "Categorieën",
            //        BorderThickness = "0,0,0,1",
            //        BorderMargin = "12,6,12,0"
            //    });
            //this.Menu.Add(new Menu()
            //    {
            //        Index = 2,
            //        Name = "Gratis",
            //        BorderThickness = "0,0,0,1",
            //        BorderMargin = "12,0,12,0"
            //    });
            //this.Menu.Add(new Menu()
            //    {
            //        Index = 3,
            //        Name = "Festivals",
            //        BorderThickness = "0,0,0,0",
            //        BorderMargin = "12,0,12,6"
            //    });

            Messenger.Default.Register<NotificationMessage>(this, async (message) =>
            {
                if (message.Notification.Equals("MainPageLoaded", StringComparison.OrdinalIgnoreCase))
                {
                    this.InitializeCategories();
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ChangePivotItems"));
                }
            });

            this.MenuCommand = new RelayCommand<string>(selectedButton =>
            {
                switch (selectedButton)
                {
                    case "Categorieën":
                        this.CategoryPivotVisibility = true;
                        this.InitializeDays();
                        ThreadPool.QueueUserWorkItem(state => this.InitializeEvents());
                        Messenger.Default.Send<PivotChangeMessage>(new PivotChangeMessage(MainPagePivotEnum.CategoryPivot.ToString(), true));
                        break;
                    default:
                        break;
                }
            });

            this.CategoryCommand = new RelayCommand<string>(async selectedButton =>
                {                    
                    this.SelectedCategory = (from item in this.Categories
                                             where item.Category.id.Equals(selectedButton)
                                             select item).First().Category;
                    this.DayPivotVisibility = true;
                    
                    Messenger.Default.Send<PivotChangeMessage>(new PivotChangeMessage(MainPagePivotEnum.DayPivot.ToString(), true));
                });

            this.DayCommand = new RelayCommand<string>(async selectedButton =>
                {
                    while (_allGFEvents.Count == 0)
                        Thread.Sleep(1);

                    _selectedDayTimestamp = selectedButton;
                    _selectedDay = (from item in _days
                                    where item.Day.timestamp == int.Parse(_selectedDayTimestamp)
                                    select item.Day.day).First();

                    var selectedEvents = (from item in _allGFEvents
                                          where item.cat_id == SelectedCategory.id
                                          && item.datum == _selectedDayTimestamp.ToString()
                                          orderby item.sort
                                          select item).ToList();

                    int itemCount = 0;
                    this.Events.Clear();
                    foreach (GFEvent gfEvent in selectedEvents)
                    {
                        ++itemCount;
                        if (itemCount == 1)
                        {
                            this.Events.Add(new EventListItem(gfEvent, "0,0,0,1", "12,6,12,0"));
                            continue;
                        }

                        if (itemCount == selectedEvents.Count)
                        {
                            this.Events.Add(new EventListItem(gfEvent, "0,0,0,0", "12,0,12,6"));
                            continue;
                        }

                        this.Events.Add(new EventListItem(gfEvent, "0,0,0,1", "12,0,12,0"));
                    }
                    this.EventPivotVisibility = true;
                    Messenger.Default.Send<PivotChangeMessage>(new PivotChangeMessage(MainPagePivotEnum.EventPivot.ToString(), true));
                }
            );

            this.EventCommand = new RelayCommand<string>(selectedButton =>
                {
                    var selectedEvent = (from item in _allGFEvents
                                         where item.id == selectedButton
                                         select item).First();

                    _navigationService.NavigateTo(ViewModelLocator.EventPageUri);
                    MessengerInstance.Send(new SelectedEventMessage(selectedEvent, _selectedDay));
                });

            this.AboutCommand = new RelayCommand(() => _navigationService.NavigateTo(ViewModelLocator.InfoPageUri));
        }

        private async Task InitializeCategories()
        {
            if (ReferenceEquals(Categories, null) || Categories.Count == 0)
            {
                string path = string.Format(@"{0}\resources\GFCategories.json", _installFolder);
                StorageFile storageFile = await StorageFile.GetFileFromPathAsync(path);
                Stream stream = await storageFile.OpenStreamForReadAsync();
                string jsonContent;
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    jsonContent = streamReader.ReadToEnd();
                }

                List<GFCategory> gfCategories = GFService.GetInstance().ParseCategories(jsonContent);

                int itemCount = 0;
                foreach (GFCategory gfCategory in gfCategories)
                {
                    ++itemCount;
                    if (itemCount == 1)
                    {
                        this.Categories.Add(new CategoryListItem(gfCategory, "0,0,0,1", "12,6,12,0"));
                        continue;
                    }

                    if (itemCount == gfCategories.Count)
                    {
                        this.Categories.Add(new CategoryListItem(gfCategory, "0,0,0,0", "12,0,12,6"));
                        continue;
                    }

                    this.Categories.Add(new CategoryListItem(gfCategory, "0,0,0,1", "12,0,12,0"));
                }
            }
        }

        private async Task InitializeEvents()
        {
            if (ReferenceEquals(_allGFEvents, null) || _allGFEvents.Count == 0)
            {
                string path = string.Format(@"{0}\resources\GFEvents.json", _installFolder);
                StorageFile storageFile = await StorageFile.GetFileFromPathAsync(path);
                Stream stream = await storageFile.OpenStreamForReadAsync();
                string jsonContent;
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    jsonContent = streamReader.ReadToEnd();
                }

                _allGFEvents = GFService.GetInstance().ParseEvents(jsonContent);
            }
        }

        private async Task InitializeDays()
        {
            if (ReferenceEquals(Days, null) || Days.Count == 0)
            {
                string path = string.Format(@"{0}\resources\GFDates.json", _installFolder);
                StorageFile storageFile = await StorageFile.GetFileFromPathAsync(path);
                Stream stream = await storageFile.OpenStreamForReadAsync();
                string jsonContent;
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    jsonContent = streamReader.ReadToEnd();
                }

                List<GFDay> gfDays = GFService.GetInstance().ParseDays(jsonContent);

                int itemCount = 0;
                foreach (GFDay gfDay in gfDays)
                {
                    ++itemCount;
                    if (itemCount == 1)
                    {
                        this.Days.Add(new DayListItem(gfDay, "0,0,0,1", "12,6,12,0"));
                        continue;
                    }

                    if (itemCount == gfDays.Count)
                    {
                        this.Days.Add(new DayListItem(gfDay, "0,0,0,0", "12,0,12,6"));
                        continue;
                    }

                    this.Days.Add(new DayListItem(gfDay, "0,0,0,1", "12,0,12,0"));
                }
            }
        }
    }
}