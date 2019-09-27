using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TrustFrontend;
using TrustFrontend.Droid;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace TrustFrontend.Droid
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        public BorderlessEntryRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
#pragma warning disable CS0618 // Тип или член устарел
                Control.Background = Resources.GetDrawable(Resource.Drawable.BorderlessEntryDrawable);
#pragma warning restore CS0618 // Тип или член устарел
                Control.SetCursorVisible(true);
                Control.SetPadding(5, 1, 1, 1);
            }
        }
    }
}