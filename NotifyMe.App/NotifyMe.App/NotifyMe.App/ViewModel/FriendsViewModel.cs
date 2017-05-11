using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotifyMe.App.Enumerations;
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
            IFacebookService fbservice,
			IApplicationCache cache,
            IMobileCenterLogger logger) : base(navService, logger)
        {
            FacebookService = fbservice;
			Cache = cache;
        }

        protected IFacebookService FacebookService { get; private set; }

		protected IApplicationCache Cache { get; private set; }

        private ICommand getFriendsCommand;

        private FacebookFriend selectedFriend;

        public FacebookFriend SelectedFriend
        {
            get { return selectedFriend; }
            set
            {
				selectedFriend = value;
				RaisePropertyChanged();

                if (value != null)
                {
					Cache.SelectedFriend = SelectedFriend;
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
                    Logger.TrackEvent(UserName, EventType.FriendsCollected);
                    IsBusy = false;
                }));
            }
        }

		public override void OnAppear()
		{
			SelectedFriend = null;
		}
    }
}
