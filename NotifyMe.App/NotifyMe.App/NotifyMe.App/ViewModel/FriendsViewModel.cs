using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotifyMe.App.Infrastructure;
using NotifyMe.App.Infrastructure.Messages;
using NotifyMe.App.Models;
using NotifyMe.App.Services;
using NotifyMe.App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotifyMe.App.ViewModel
{
    public class FriendsViewModel : BaseViewModel
    {
        public FriendsViewModel(
            INavigationService navService,
            IFacebookService fbservice) : base(navService)
        {
            FacebookService = fbservice;
        }

        protected IFacebookService FacebookService { get; private set; }

        private ICommand getFriendsCommand;

        private FacebookFriend selectedFriend;

        public FacebookFriend SelectedFriend
        {
            get { return selectedFriend; }
            set
            {
                if (value != null)
                {
                    NavigationService.NavigateTo(ViewTypes.MessageCreate, value);
                }
            }
        }

        private ObservableCollection<FacebookFriend> friends;

        public ObservableCollection<FacebookFriend> Friends
        {
            get { return friends; }
            set { friends = value; RaisePropertyChanged(); }
        }

        public ICommand GetFriendsCommand
        {
            get
            {
                return getFriendsCommand ?? (getFriendsCommand = new RelayCommand(async () =>
                {
                    IsBusy = true;
                    Friends = new ObservableCollection<FacebookFriend>(await FacebookService.GetFacebookFriends());
                    IsBusy = false;
                }));
            }
        }

        public async Task SendTestMessage()
        {
            Message message = new Message();
            message.Body = "Wiadomość testowa";
            message.From = MobileServiceClientWrapper.Instance.CurrentUser.Name;
            message.RecipientId = SelectedFriend.Id;

            var serialized = JsonConvert.SerializeObject(message);

            var jtoken = JToken.Parse(serialized);

            try
            {
                var result = await MobileServiceClientWrapper.Instance.Client.InvokeApiAsync("notifications/send", jtoken, HttpMethod.Post, null);
                Messenger.Default.Send<SendedMessage>(new SendedMessage() { RecipientName = SelectedFriend.Name });
            }
            catch (Exception ex)
            {

            }

        }
    }
}
