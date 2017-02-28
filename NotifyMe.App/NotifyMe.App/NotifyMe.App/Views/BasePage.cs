using NotifyMe.App.ViewModel;
using Xamarin.Forms;

namespace NotifyMe.App.Views
{
    public class BasePage<TViewModel> : ContentPage where TViewModel : BaseViewModel
    {
        private readonly TViewModel viewModel;

        public TViewModel ViewModel
        {
            get { return viewModel; }
        }

        public BasePage()
        {
            viewModel = App.Container.GetInstanceWithoutCaching<TViewModel>();
            BindingContext = viewModel;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnBack();
        }
    }
}
