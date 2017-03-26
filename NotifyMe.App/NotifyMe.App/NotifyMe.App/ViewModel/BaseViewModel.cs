using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

namespace NotifyMe.App.ViewModel
{
	public class BaseViewModel : ViewModelBase
	{
		public BaseViewModel(INavigationService navigationService)
		{
			NavigationService = navigationService;
		}

		protected INavigationService NavigationService { get; private set; }

		private bool isBusy;

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

		public virtual void OnBack()
		{
			Messenger.Default.Unregister(this);
		}

		public virtual void OnAppear()
		{

		}
	}
}
