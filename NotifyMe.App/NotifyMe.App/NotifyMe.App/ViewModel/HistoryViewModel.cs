using NotifyMe.App.Models.Entities;
using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Views;

namespace NotifyMe.App.ViewModel
{
    public class HistoryViewModel : BaseViewModel
    {
        private ObservableCollection<SentMessage> sentMessages;

        public HistoryViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            SentMessages = new ObservableCollection<SentMessage>();
            SentMessages.Add(new SentMessage() { Body = "Message...", Receiver = "John Doe", Date = DateTime.Now.AddHours(1).ToString() });
            SentMessages.Add(new SentMessage() { Body = "Message...", Receiver = "Michael Douglas", Date = DateTime.Now.AddDays(-1).ToString() });
            SentMessages.Add(new SentMessage() { Body = "Message...", Receiver = "Miroslav Francesc", Date = DateTime.Now.ToString() });
            SentMessages.Add(new SentMessage() { Body = "Message...", Receiver = "Vlad Cornelius", Date = DateTime.Now.AddDays(-2).AddHours(-3).ToString() });
        }

        public ObservableCollection<SentMessage> SentMessages
        {
            get { return sentMessages; }
            set { sentMessages = value; }
        }
    }
}
