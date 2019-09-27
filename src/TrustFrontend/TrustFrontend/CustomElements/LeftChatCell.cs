using Xamarin.Forms;

namespace TrustFrontend
{
    public class LeftChatCell : ContentView
	{
        #region LeftChatCell's views
        private Label messageTextLabel;
        private Label messageTimeLabel;
        private StackLayout messageLayout;
        #endregion
        #region Bindable properties
        public readonly BindableProperty MessageTextBindable = BindableProperty.Create(
            "MessageText", typeof(string), typeof(LeftChatCell));
        public string MessageText
        {
            get => GetValue(MessageTextBindable) as string;
            set
            {
                SetValue(MessageTextBindable, value);
                messageTextLabel.Text = value;
            }
        }

        public readonly BindableProperty MessageTimeBindable = BindableProperty.Create(
            "MessageTime", typeof(string), typeof(LeftChatCell));
        public string MessageTime
        {
            get => GetValue(MessageTimeBindable) as string;
            set
            {
                SetValue(MessageTimeBindable, value);
                messageTimeLabel.Text = value;
            }
        }
        #endregion
        public LeftChatCell()
		{
            StackLayout backgroundLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.LightCyan
            };
            Frame messageFrame = new Frame
            {
                Padding = 0,
                BorderColor = Color.White,
                BackgroundColor = Color.White,
                CornerRadius = 15,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(80, 0, 10, 0)
            };
            messageLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(10, 0, 0, 0),
                BackgroundColor = Color.White
            };
            messageTextLabel = new Label
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontFamily = "Arial, Verdana",
                FontSize = 14,
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Start,
                Margin = new Thickness(5, 5, 10, 0)
            };
            messageTimeLabel = new Label
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontFamily = "Arial, Verdana",
                FontSize = 11,
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Start,
                Margin = new Thickness(5, 5, 10, 5)
            };
            messageLayout.Children.Add(messageTextLabel);
            messageLayout.Children.Add(messageTimeLabel);
            messageFrame.Content = messageLayout;
            backgroundLayout.Children.Add(messageFrame);
            Content = backgroundLayout;
		}
	}
}