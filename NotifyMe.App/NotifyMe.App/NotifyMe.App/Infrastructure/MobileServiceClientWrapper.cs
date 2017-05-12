using Microsoft.WindowsAzure.MobileServices;
using NotifyMe.App.Models;

namespace NotifyMe.App.Infrastructure
{
    public class MobileServiceClientWrapper
    {
        private static MobileServiceClientWrapper instance = null;
        private static readonly object padLock = new object();
        private MobileServiceClient client;

        public static MobileServiceClientWrapper Instance
        {
            get
            {
                lock (padLock)
                {
                    if (instance == null)
                    {
                        instance = new MobileServiceClientWrapper();
                    }
                    return instance;
                }
            }
        }

        public MobileServiceClient Client
        {
            get { return client; }
        }

        public ApplicationUser CurrentUser { get; set; }

        public MobileServiceClientWrapper()
        {
            client = new MobileServiceClient(@"https://notifymeradek.azurewebsites.net");
        }
    }
}
