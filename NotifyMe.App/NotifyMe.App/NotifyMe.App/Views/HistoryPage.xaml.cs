using NotifyMe.App.ViewModel;

namespace NotifyMe.App.Views
{
    public partial class HistoryPage : BasePage<HistoryViewModel>
    {
        public HistoryPage()
        {
            InitializeComponent();
            historyList.ItemSelected += (sender, e) =>
            {
                historyList.SelectedItem = false;
            };
        }
    }
}
