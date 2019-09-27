using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace TrustFrontend
{
    public class ListViewGrouping<K, T> : ObservableCollection<T>
    {
        public K Name { get; set; }
        public ListViewGrouping(K name, IEnumerable<T> items)
        {
            Name = name; 
            foreach (T item in items)
            {
                Items.Add(item);
            }
        }
    }
}
