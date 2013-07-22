using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Tasks;

namespace AppCreativity.GentseFeesten.WP8.Framework
{
    public static class Helpers
    {
        public static void GetItemsRecursive<T>(DependencyObject lb, ref List<T> objectList) where T : DependencyObject
        {
            List<DependencyObject> listDepOb = new List<DependencyObject>();
            var childrenCount = VisualTreeHelper.GetChildrenCount(lb);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(lb, i);

                if (child is T)
                {
                    objectList.Add(child as T);
                }

                GetItemsRecursive<T>(child, ref objectList);
            }

            return;
        }

        public static void NavigateToWebsite(string uri)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(uri);
            webBrowserTask.Show();
        }
    }
}
