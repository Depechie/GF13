using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AppCreativity.GentseFeesten.WP8.Framework;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AppCreativity.GentseFeesten.WP8.View
{
    public partial class InfoPage : PhoneApplicationPage
    {
        public InfoPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Helpers.NavigateToWebsite("https://gf13.uservoice.com/");
        }
    }
}