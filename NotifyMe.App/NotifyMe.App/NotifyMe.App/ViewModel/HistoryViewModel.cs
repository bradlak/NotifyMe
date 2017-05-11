using NotifyMe.App.Models.Entities;
using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Views;
using NotifyMe.App.Services;
using System.Linq;
using NotifyMe.App.Infrastructure.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace NotifyMe.App.ViewModel
{
    public class HistoryViewModel : BaseViewModel
    {
        private ObservableCollection<SentMessage> sentMessages;

        private SentMessage selectedMessage;

        public HistoryViewModel(
            IDatabaseService dbService,
            INavigationService navigationService,
            IMobileCenterLogger logger) 
            : base(navigationService, logger)
        {
            DatabaseService = dbService;
            LoadHistory();
            Messenger.Default.Register<RefreshHistoryMessage>(this, RefreshHistoryHandler);
        }

        protected IDatabaseService DatabaseService { get; private set; }

        public ObservableCollection<SentMessage> SentMessages
        {
            get { return sentMessages; }
            set { sentMessages = value; RaisePropertyChanged(); }
        }

        public SentMessage SelectedMessage
        {
            get { return selectedMessage; }
            set { selectedMessage = value; }
        }

        public void LoadHistory()
        {
            var data = DatabaseService.GetAll<SentMessage>().OrderByDescending(z => DateTime.Parse(z.Date));
            SentMessages = new ObservableCollection<SentMessage>(data);
        }
        private void RefreshHistoryHandler(RefreshHistoryMessage obj)
        {
            LoadHistory();
        }

        public override void OnBack()
        {
        }
    }
}
