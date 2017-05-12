using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using NotifyMe.App.Enumerations;
using NotifyMe.App.Models;
using NotifyMe.App.Services;
using NotifyMe.App.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NotifyMe.App.ViewModel
{
    public class FriendsViewModel : BaseViewModel
    {
        public FriendsViewModel(
            INavigationService navService,
            IApplicationCache cache,
            IFacebookService fbservice,
            IMobileCenterLogger logger) : base(navService, cache, logger)
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
                selectedFriend = value;
                RaisePropertyChanged();

                if (value != null)
                {
                    ApplicationCache.SelectedFriend = SelectedFriend;
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
