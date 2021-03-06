﻿using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using NotifyMe.App.Infrastructure;
using NotifyMe.App.Resources;
using NotifyMe.App.Services;
using NotifyMe.App.ViewModel;
using NotifyMe.App.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NotifyMe.App
{
    public partial class App : Application
    {
        public static SimpleIoc Container { get; set; }

        public App()
        {
            InitializeComponent();

            Container = SimpleIoc.Default;

            var navigationService = new NavigationService();
            navigationService.Configure(ViewTypes.LoginPage, typeof(LoginPage));
            navigationService.Configure(ViewTypes.FriendsPage, typeof(FriendsPage));
            navigationService.Configure(ViewTypes.MessageCreate, typeof(CreateMessagePage));
            navigationService.Configure(ViewTypes.MainPage, typeof(MainPage));

            Container.Register<LoginViewModel>();
            Container.Register<FriendsViewModel>();
            Container.Register<CreateMessageViewModel>();
            Container.Register<HistoryViewModel>();

			if (!Container.IsRegistered<INavigationService>())
			{
				Container.Register<INavigationService>(() => navigationService);
			}

            Container.Register<IFacebookService, FacebookService>();
			Container.Register<IApplicationCache, ApplicationCache>();
            Container.Register<IDatabaseService, DatabaseService>();
            Container.Register<IMobileCenterLogger, MobileCenterLogger>();

            Container.Register<LoginPage>();
            Container.Register<FriendsPage>();
            Container.Register<CreateMessagePage>();
            Container.Register<MainPage>();
            Container.Register<HistoryPage>();

            var mainPage = new NavigationPage(new LoginPage());
			mainPage.BackgroundColor = Colors.Background;

			navigationService.Initialize(mainPage);

            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            MobileCenter.Start("android=your key" +
                   "ios=your key",
                   typeof(Analytics), typeof(Crashes));
        }
    }
}
