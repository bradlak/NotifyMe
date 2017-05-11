using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using NotifyMe.App.Infrastructure;
using NotifyMe.App.Services;

namespace NotifyMe.App.ViewModel
{
	public class BaseViewModel : ViewModelBase
	{
        private bool isBusy;

        public BaseViewModel(
            INavigationService navigationService,
            IMobileCenterLogger logger)
		{
			NavigationService = navigationService;
            Logger = logger;
		}

		protected INavigationService NavigationService { get; private set; }

        protected IMobileCenterLogger Logger { get; private set; }

		public bool IsBusy
		{
			get
			{
				return isBusy;

			}
			set
			{
				isBusy = value;
				RaisePropertyChanged();
			}
		}

        public string UserName => MobileServiceClientWrapper.Instance.CurrentUser.Name;

		public virtual void OnBack()
		{
			Messenger.Default.Unregister(this);
		}

		public virtual void OnAppear()
		{

		}
	}
}
