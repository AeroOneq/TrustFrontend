using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TrustFrontend
{
    public class MenuItem
    {
        public MenuItem()
        {
            TargetType = typeof(MainPageDetail);
        }
        public int Id { get; set; }
        public string MenuItemText { get; set; }
        public ImageSource MenuItemIconSource { get; set; }

        public Type TargetType { get; set; }
    }
}
