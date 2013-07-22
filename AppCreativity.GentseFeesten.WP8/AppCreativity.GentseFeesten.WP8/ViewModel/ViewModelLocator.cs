using System;
using AppCreativity.GentseFeesten.WP8.Messages;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;

namespace AppCreativity.GentseFeesten.WP8.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public static readonly Uri EventPageUri = new Uri("/View/EventPage.xaml", UriKind.Relative);
        public static readonly Uri InfoPageUri = new Uri("/View/InfoPage.xaml", UriKind.Relative);        

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<IMessenger, LastMessageReplayMessenger>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<EventViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public EventViewModel Event
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EventViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}