using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ServerLib;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrustFrontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContractChatPage : ContentPage
    {
        #region Chat editor background layout constants
        private const int InitialChatEditorHeight = 65;
        private const int MaximumChatEditorHeight = 195;
        private const int ChatEditorHeightDelta = 10;
        private const int MaximumNumberOfLines = 14;
        #endregion

        #region Properties
        private ContractInfo CurrentContract { get; set; }
        private UserInfo CurrentUser { get; set; }
        private CosmosDB CosmosDB { get; } = CosmosDBSingleton.GetObj();
        private int NumberOfMessages { get; set; }
        private List<MessageInfo> Messages { get; set; } = new List<MessageInfo>();
        private double MaxScrollY { get; set; }
        private bool DoWeNeedToScrollDown { get; set; } = false;
        private bool IsUpdateProcessOn { get; set; } = true;
        private CancellationTokenSource TokenSource { get; set; }
        private CancellationToken CancellationToken { get; set; }
        #endregion

        public ContractChatPage(ContractInfo contract, UserInfo user)
        {
            InitializeComponent();
            TokenSource = new CancellationTokenSource();
            CancellationToken = TokenSource.Token;
            SetPropertiesValue(contract, user);
            DoInitialUploadAsync(contract, user);
        }

        #region Main methods
        private async void DoInitialUploadAsync(ContractInfo contract, UserInfo user)
        {
            SwitchOnActivityIndicator();
            await CosmosDB.Connect();
            List<MessageInfo> messages = await CosmosDB.GetAllMessages(contract);
            UploadChatToUI(messages, user.Id, 0);
            await chatScrollView.ScrollToAsync(messagesLayout, ScrollToPosition.End, false);
            MaxScrollY = chatScrollView.ScrollY;
            SwitchOffActivityIndicator();
            SwitchListenOn();
        }
        private void SwitchListenOn()
        {
            StartTimer();
            Task.Run(() =>
            {
                try
                {
                    Update();
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Exception");
                }
            });
        }
        private void SendMessage(ContractInfo contract, UserInfo user)
        {
            try
            {
                if (messageEditor.Text != null && messageEditor.Text != string.Empty)
                {
                    MessageInfo message = CreateMessageObject(contract, user);
                    AddMessageToTheCosmosDB(message);
                    DoWeNeedToScrollDown = true;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }
        private void StartTimer()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(250),() =>
            {
                try
                {
                    if (Messages.Count > 0 && Messages.Count != NumberOfMessages)
                    {
                        MessageInfo[] messagesArray = new MessageInfo[Messages.Count];
                        Messages.CopyTo(messagesArray);
                        UploadChatToUI(messagesArray.ToList(), CurrentUser.Id, NumberOfMessages);
                        DoAfterChatUpdateActions();
                    }
                    return IsUpdateProcessOn;
                }
                catch (Exception ex)
                {
                    DisplayAlert("Ошибка", ex.Message, "OK");
                    return false;
                }
            });
        }
        #endregion
        #region Chat updarte methods
        private async void Update()
        {
            try
            {
                while (true)
                {
                    if (CancellationToken.IsCancellationRequested)
                        break;
                    Messages = await CosmosDB.GetAllMessages(CurrentContract);
                    await Task.Delay(350);
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Ошибка", "Обновление невозможно, перезайдите на страницу", "OK");
            }
        }
        private void UploadChatToUI(List<MessageInfo> messages, int userId,
            int startIndex)
        {
            NumberOfMessages = messages.Count;
            for (int i = startIndex; i < messages.Count; i++)
                if (messages[i].UserId == userId)
                    messagesLayout.Children.Add(new LeftChatCell
                    {
                        MessageText = messages[i].MessageText,
                        MessageTime = messages[i].MessageSendDate.ToShortDateString() + " "
                            + messages[i].MessageSendDate.ToShortTimeString(),
                        Margin = new Thickness(80, 10, 10, i == messages.Count - 1 ? 10 : 0)
                    });
                else
                    messagesLayout.Children.Add(new RightChatCell
                    {
                        MessageText = messages[i].MessageText,
                        MessageTime = messages[i].MessageSendDate.ToShortDateString() + " "
                            + messages[i].MessageSendDate.ToShortTimeString(),
                        MessageAuthor = messages[i].AuthorName,
                        Margin = new Thickness(10, 10, 80, i == messages.Count - 1 ? 10 : 0)
                    });
        }
        #endregion
        #region Utility methods
        private void ChangeSizeOfChatBackLayout(int numberOfNewLines, int numberOfOldLines)
        {
            int delta = numberOfNewLines - numberOfOldLines;
            if (delta < 0)
            {
                if (numberOfNewLines <= MaximumNumberOfLines &&
                    chatEntryBackgroundLayout.HeightRequest - ChatEditorHeightDelta
                    >= InitialChatEditorHeight)
                {
                    chatEntryBackgroundLayout.HeightRequest = InitialChatEditorHeight
                        + numberOfNewLines * ChatEditorHeightDelta;
                }
            }
            else
            {
                if (chatEntryBackgroundLayout.HeightRequest + ChatEditorHeightDelta * delta
                    <= MaximumChatEditorHeight)
                {
                    chatEntryBackgroundLayout.HeightRequest += ChatEditorHeightDelta * delta;
                }
                else
                    chatEntryBackgroundLayout.HeightRequest = MaximumChatEditorHeight;
            }
        }

        private void DoAfterChatUpdateActions()
        {
            if (getDownBtn.IsVisible)
                newMessagesNotificationBtn.IsVisible = true;
            if (DoWeNeedToScrollDown)
            {
                chatScrollView.ScrollToAsync(messagesLayout, ScrollToPosition.End, false);
                DoWeNeedToScrollDown = false;
            }
        }

        private void SwitchOnActivityIndicator()
        {
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;
        }

        private void SwitchOffActivityIndicator()
        {
            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }

        private void SetPropertiesValue(ContractInfo contract, UserInfo user)
        {
            CurrentContract = contract;
            CurrentUser = user;
        }

        private void AddMessageToTheCosmosDB(MessageInfo message)
        {
            CosmosDB.InsertMessageIntoTheDB(message);
        }

        private MessageInfo CreateMessageObject(ContractInfo contract,
            UserInfo user)
        {
            string text = messageEditor.Text;
            DateTime sendTime = DateTime.Now;
            string id = DateTime.Now.ToBinary().ToString();
            int userId = user.Id;
            string chatId = $"contractChat{contract.Id}";
            string userName = user.Name + " " + user.Surname;
            return new MessageInfo(text, sendTime, id, userId, chatId,
                userName);
        }

        private int CountLines(string text)
        {
            if (text == null)
                text = string.Empty;
            int linesNum = 0;
            for (int i = 0; i < text.Length; i++)
                if (text[i] == '\n')
                    linesNum++;
            return linesNum;
        }
        #endregion
        #region Event handlers
        private void ChatBackgroundLayoutSizeChanged(object sender, EventArgs e)
        {
            StackLayout chatBackLayout = sender as StackLayout;
            getDownBtn.Margin = new Thickness(0, 0, 10, chatBackLayout.Height + 5);
            newMessagesNotificationBtn.Margin = new Thickness(0, 0, 10, 
                getDownBtn.Margin.Bottom + 23);
        }
        private void MessageEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            int numberOfNewLines = CountLines(e.NewTextValue);
            int numberOfOldLines = CountLines(e.OldTextValue);
            Thickness oldMargin = chatScrollView.Margin;
            if (numberOfNewLines != numberOfOldLines)
                ChangeSizeOfChatBackLayout(numberOfNewLines, numberOfOldLines);
        }
        private void OnChatScrollViewScroll(object sender, ScrolledEventArgs e)
        {
            if (MaxScrollY - e.ScrollY >= 200)
                getDownBtn.IsVisible = true;
            else
            {
                getDownBtn.IsVisible = false;
                newMessagesNotificationBtn.IsVisible = false;
            }
        }
        private async void GetDownButtonClick(object sender, EventArgs e)
        {
            await chatScrollView.ScrollToAsync(messagesLayout, ScrollToPosition.End, false);
            MaxScrollY = chatScrollView.ScrollY;
            newMessagesNotificationBtn.IsVisible = false;
        }
        private void SendMessageBtnClick(object sender, EventArgs e)
        {
            SendMessage(CurrentContract, CurrentUser);
            messageEditor.Text = null;
        }
        #endregion
        #region Overriden methods
        protected override bool OnBackButtonPressed()
        {
            IsUpdateProcessOn = false;
            TokenSource.Cancel();
            return base.OnBackButtonPressed();
        }
        #endregion
    }
}