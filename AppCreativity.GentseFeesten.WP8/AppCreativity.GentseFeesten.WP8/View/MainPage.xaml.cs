using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using AppCreativity.GentseFeesten.WP8.Messages;
using AppCreativity.GentseFeesten.WP8.Model;
using AppCreativity.GentseFeesten.WP8.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using System.Windows;

namespace AppCreativity.GentseFeesten.WP8.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Dictionary<string, PivotItem> _pivotDictionary = new Dictionary<string, PivotItem>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            InitializePivots();
            Messenger.Default.Register<NotificationMessage>(this, message =>
                {
                    if (message.Notification.Equals("ChangePivotItems"))
                        this.ChangePivotItems(null);
                });

            Messenger.Default.Register<PivotChangeMessage>(this, pivotChangeMessage => this.ChangePivotItems(pivotChangeMessage));

            this.Loaded += this.OnPageLoaded;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("MainPageLoaded"));            
            this.Loaded -= this.OnPageLoaded;
        }

        private void LongListSelector_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Button t = (e.OriginalSource as Border).Child as Button;
        }

        private void InitializePivots()
        {
            foreach (PivotItem item in GF13Pivot.Items.ToList())
                _pivotDictionary.Add(item.Name, item);
        }

        private void ChangePivotItems(PivotChangeMessage pivotChangeMessage)
        {
            foreach (PivotItem item in GF13Pivot.Items.ToList())
            {
                if (item.Visibility == Visibility.Collapsed)
                    GF13Pivot.Items.Remove(item);
            }

            if (!ReferenceEquals(pivotChangeMessage, null))
                if (!GF13Pivot.Items.Contains(_pivotDictionary[pivotChangeMessage.PivotName]))
                {
                    PivotItem pivotItem = _pivotDictionary[pivotChangeMessage.PivotName];
                    pivotItem.Loaded += PivotItemOnLoaded;
                    GF13Pivot.Items.Add(pivotItem);
                }
                else
                    GF13Pivot.SelectedItem = _pivotDictionary[pivotChangeMessage.PivotName];
        }

        private void PivotItemOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            PivotItem pivotItem = sender as PivotItem;
            GF13Pivot.SelectedItem = pivotItem;
            pivotItem.Loaded -= PivotItemOnLoaded;

        }
    }
}