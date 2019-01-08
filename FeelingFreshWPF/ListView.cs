using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FeelingFreshWPF
{
    public class CustomListView : ListView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is ListViewItem container && item is HelperLibrary.IIsEnabled binaryItem)
            {
                container.IsEnabled = binaryItem.IsEnabled;
                container.IsHitTestVisible = binaryItem.IsEnabled;
            }
        }
    }
}
