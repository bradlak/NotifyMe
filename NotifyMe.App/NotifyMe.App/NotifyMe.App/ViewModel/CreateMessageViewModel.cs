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

namespace NotifyMe.App.ViewModel
{
    public class CreateMessageViewModel : BaseViewModel
    {
        private string messageRecipient;

        private string messageBody;

        private ICommand sendMessageCommand;

        public CreateMessageViewModel(
            INavigationService navigationService,
            IApplicationCache cache,
            IDatabaseService dbService)
            : base(navigationService)
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
                From = MobileServiceClientWrapper.Instance.CurrentUser.Name,
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

            IsBusy = false;

            NavigationService.GoBack();
        }

        public override void OnAppear()
        {
            base.OnAppear();
            MessageRecipient = Cache.SelectedFriend.Name;
        }
    }
}
