using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Windows.Input;
using NotifyMe.App.Views;
using NotifyMe.App.Services;
using NotifyMe.App.Infrastructure;
using GalaSoft.MvvmLight.Messaging;
using NotifyMe.App.Infrastructure.Messages;
using NotifyMe.App.Enumerations;
using Xamarin.Forms;

namespace NotifyMe.App.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private ICommand loginCommand;

        public LoginViewModel(
            IFacebookService facebookService,
            IApplicationCache cache,
            INavigationService navService,
            IMobileCenterLogger logger) 
            : base(navService, cache, logger)
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
                    IsBusy = true;
                    var success = await DependencyService.Get<ILoginService>().Login();
                    if (success)
                    {
                        if (ApplicationCache.CurrentUser == null)
                        {
                            ApplicationCache.CurrentUser = await FacebookService.GetCurrentApplicationUser();
                            MobileServiceClientWrapper.Instance.CurrentUser = ApplicationCache.CurrentUser;
                        }

                        Messenger.Default.Send<RegistrationMessage>(new RegistrationMessage());
                        Logger.TrackEvent(UserName, EventType.UserLogged);
                    }

                    IsBusy = false;
                    NavigationService.NavigateTo(ViewTypes.MainPage);
                }));
            }
        }
    }
}
