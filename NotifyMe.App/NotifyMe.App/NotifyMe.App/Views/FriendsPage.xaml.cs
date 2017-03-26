using System;
using GalaSoft.MvvmLight.Messaging;
using NotifyMe.App.Infrastructure.Messages;
using NotifyMe.App.ViewModel;
using Xamarin.Forms;

namespace NotifyMe.App.Views
{
    public partial class FriendsPage : BasePage<FriendsViewModel>
    {
        public FriendsPage()
        {
            InitializeComponent();
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ListView.SelectedItem = null;
		}
    }
}
