using Xamarin.Forms;

namespace TrustFrontend
{
    class ActivityIndicatorActions
    {
        public static void SetActivityIndicatorOn(ActivityIndicator activityIndicator)
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
        }
        public static void SetActivityIndicatorOff(ActivityIndicator activityIndicator)
        {
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
        }
    }
}
