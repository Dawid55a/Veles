using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Veles_Application.Components
{
    public class ScrollingListView : ListView
    {
        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int newItemCount;
            try
            {
                newItemCount = e.NewItems.Count;

            }
            catch (NullReferenceException)
            {
                newItemCount = 0;
            }

                 

            if (newItemCount > 0)
                this.ScrollIntoView(e.NewItems[newItemCount - 1]);

            base.OnItemsChanged(e);
        }
        
        static ScrollingListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollingListView), new FrameworkPropertyMetadata(typeof(ScrollingListView)));
        }
    }
}
