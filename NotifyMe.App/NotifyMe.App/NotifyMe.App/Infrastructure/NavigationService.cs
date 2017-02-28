using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using NotifyMe.App.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotifyMe.App.Infrastructure
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<string, Type> registeredPages = new Dictionary<string, Type>();
        private NavigationPage navigation;

        public string CurrentPageKey
        {
            get
            {
                if (navigation.CurrentPage == null)
                {
                    return null;
                }

                var pageType = navigation.CurrentPage.GetType();

                return registeredPages.ContainsValue(pageType)
                    ? registeredPages.First(p => p.Value == pageType).Key
                    : null;
            }
        }

        public void GoBack()
        {
            navigation.PopAsync();
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            if (registeredPages.ContainsKey(pageKey))
            {
                var type = registeredPages[pageKey];

                var page = App.Container.GetInstance(type, Guid.NewGuid().ToString());
                var pageType = page.GetType();
                // fix with reflection
                navigation.PushAsync(page as Page);
                Messenger.Default.Send(new NavigationMessage(parameter));
            }
        }

        public void Configure(string pageKey, Type pageType)
        {
            if (registeredPages.ContainsKey(pageKey))
            {
                registeredPages[pageKey] = pageType;
            }
            else
            {
                registeredPages.Add(pageKey, pageType);
            }
        }

        public void Initialize(NavigationPage navigation)
        {
            this.navigation = navigation;
        }
    }
}
