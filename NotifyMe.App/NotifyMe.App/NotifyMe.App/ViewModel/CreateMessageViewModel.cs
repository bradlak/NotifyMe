using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotifyMe.App.Infrastructure;
using NotifyMe.App.Infrastructure.Messages;
using NotifyMe.App.Models;

namespace NotifyMe.App.ViewModel
{
    public class CreateMessageViewModel : BaseViewModel
    {
		private string messageRecipient;

		private string messageBody;

		private ICommand sendMessageCommand;

        public CreateMessageViewModel(
            INavigationService navigationService,
			IApplicationCache cache) 
            : base(navigationService)
        {
			Cache = cache;
        }

		protected IApplicationCache Cache { get; private set; }


		public string MessageRecipient
		{
			get
			{
				return messageRecipient;
			}
			set
			{
				messageRecipient = value;
				RaisePropertyChanged();
			}
		}

		public string MessageBody
		{
			get
			{
				return messageBody;
			}
			set
			{
				messageBody = value;
				RaisePropertyChanged();
			}
		}

		public ICommand SendMessageCommand
		{
			get 
			{
				return sendMessageCommand ?? (sendMessageCommand = new RelayCommand(async () =>
				{
					await SendMessage();
				}));
			}
		}

		private async Task SendMessage()
		{
			Message message = new Message();
			message.Body = MessageBody;
			message.From = MobileServiceClientWrapper.Instance.CurrentUser.Name;
			message.RecipientId = Cache.SelectedFriend.Id;

			var serialized = JsonConvert.SerializeObject(message);

			var jtoken = JToken.Parse(serialized);

			try
			{
				IsBusy = true;
				var result = await MobileServiceClientWrapper.Instance.Client.InvokeApiAsync("notifications/send", jtoken, HttpMethod.Post, null);
				IsBusy = false;
				NavigationService.GoBack();
			}
			catch (Exception ex)
			{

			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			MessageRecipient = Cache.SelectedFriend.Name;
		}
    }
}
