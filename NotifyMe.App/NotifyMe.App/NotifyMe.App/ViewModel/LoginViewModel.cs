using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Windows.Input;
using NotifyMe.App.Views;
using NotifyMe.App.Services;
using NotifyMe.App.Infrastructure;
using GalaSoft.MvvmLight.Messaging;
using NotifyMe.App.Infrastructure.Messages;
using NotifyMe.App.Enumerations;

namespace NotifyMe.App.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private ICommand loginCommand;

        public LoginViewModel(
            IFacebookService facebookService,
            INavigationService navService,
            IMobileCenterLogger logger) 
            : base(navService, logger)
        {
            FacebookService = facebookService;
        }

        protected IFacebookService FacebookService { get; private set; }

        public ICommand LoginCommand
        {
            get
            {
                return loginCommand ?? (loginCommand = new RelayCommand(async () =>
                {
                    var success = await App.Authenticator.Authenticate();
                    if (success)
                    {
                        if (MobileServiceClientWrapper.Instance.CurrentUser == null)
                        {
                            MobileServiceClientWrapper.Instance.CurrentUser = await FacebookService.GetCurrentApplicationUser();
                        }

                        Messenger.Default.Send<RegistrationMessage>(new RegistrationMessage());
                        Logger.TrackEvent(UserName, EventType.UserLogged);

                        NavigationService.NavigateTo(ViewTypes.MainPage);
                    }
                }));
            }
        }
    }
}
