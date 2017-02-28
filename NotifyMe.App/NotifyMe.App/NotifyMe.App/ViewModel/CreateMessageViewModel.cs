using GalaSoft.MvvmLight.Views;
using NotifyMe.App.Infrastructure.Messages;

namespace NotifyMe.App.ViewModel
{
    public class CreateMessageViewModel : BaseViewModel
    {
        public CreateMessageViewModel(
            INavigationService navigationService) 
            : base(navigationService)
        {
        }

        protected override void NavigatedToHandler(NavigationMessage obj)
        {
            
        }
    }
}
