using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotifyMe.App.Infrastructure;
using NotifyMe.App.Infrastructure.Messages;
using NotifyMe.App.Models;
using NotifyMe.App.Services;
using NotifyMe.App.Models.Entities;
using GalaSoft.MvvmLight.Messaging;
using NotifyMe.App.Enumerations;

namespace NotifyMe.App.ViewModel
{
    public class CreateMessageViewModel : BaseViewModel
    {
        private string messageRecipient;

        private string messageBody;

        private bool messageSent;

        private ICommand sendMessageCommand;

        public CreateMessageViewModel(
            INavigationService navigationService,
            IApplicationCache cache,
            IDatabaseService dbService,
            IMobileCenterLogger logger)
            : base(navigationService, logger)
        {
            Cache = cache;
            DatabaseService = dbService;
        }

        protected IApplicationCache Cache { get; private set; }

        protected IDatabaseService DatabaseService { get; private set; }

        public string MessageRecipient
        {
            get
            {
                return messageRecipient;
            }
            set
            {
                messageRecipient = value;
                RaisePropertyChanged();
            }
        }

        public string MessageBody
        {
            get
            {
                return messageBody;
            }
            set
            {
                messageBody = value;
                RaisePropertyChanged();
            }
        }

        public bool MessageSent
        {
            get
            {
                return messageSent;
            }
            set
            {
                messageSent = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SendMessageCommand
        {
            get
            {
                return sendMessageCommand ?? (sendMessageCommand = new RelayCommand(async () =>
                {
                    await SendMessage();
                }));
            }
        }

        private async Task SendMessage()
        {
            IsBusy = true;

            Message message = new Message()
            {
                Body = MessageBody,
                From = UserName,
                RecipientId = Cache.SelectedFriend.Id
            };

            var serialized = JsonConvert.SerializeObject(message);
            var jtoken = JToken.Parse(serialized);
            try
            {
                var result = await MobileServiceClientWrapper.Instance.Client.InvokeApiAsync("notifications/send", jtoken, HttpMethod.Post, null);
            }
            catch (Exception ex)
            {
                MessageRecipient = "Not found";
                await Task.Delay(1000);
            }

            DatabaseService.Add<SentMessage>(new SentMessage(Cache.SelectedFriend.Name, MessageBody, DateTime.Now.ToString()));
            Messenger.Default.Send<RefreshHistoryMessage>(new RefreshHistoryMessage());
            Logger.TrackEvent(UserName, EventType.MessageSent);

            IsBusy = false;
            MessageSent = true;
        }

        public override void OnAppear()
        {
            base.OnAppear();
            MessageRecipient = Cache.SelectedFriend.Name;
        }

        public void Close()
        {
            NavigationService.GoBack();
        }
    }
}
