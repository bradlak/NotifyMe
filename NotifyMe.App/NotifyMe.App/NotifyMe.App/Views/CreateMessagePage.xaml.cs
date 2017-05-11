using NotifyMe.App.ViewModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotifyMe.App.Views
{
    public partial class CreateMessagePage : BasePage<CreateMessageViewModel>
    {
        public CreateMessagePage()
        {
            InitializeComponent();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private async void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.MessageSent))
            {
                await Task.WhenAll(
                    envelope.TranslateTo(300, -300, 1500, Easing.Linear),
                     envelope.ScaleTo(0.2, 1500, Easing.Linear)
                );
                ViewModel.Close();
            }
        }
    }
}
