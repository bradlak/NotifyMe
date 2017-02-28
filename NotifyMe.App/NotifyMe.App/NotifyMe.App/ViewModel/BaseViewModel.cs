using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using NotifyMe.App.Infrastructure.Messages;
using Xamarin.Forms;

namespace NotifyMe.App.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            Messenger.Default.Register<NavigationMessage>(this, this.GetType(), NavigatedToHandler);
        }

        protected virtual void NavigatedToHandler(NavigationMessage obj)
        {
            
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
    }
}
