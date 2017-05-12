using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NotifyMe.App.Infrastructure;
using NotifyMe.App.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IosLoginService))]
namespace NotifyMe.App.iOS
{
    public class IosLoginService : ILoginService
    {
        private MobileServiceClient client;

        public IosLoginService()
        {
            client = MobileServiceClientWrapper.Instance.Client;
        }

        public async Task<bool> Login()
        {
            var user = await client.LoginAsync(
                UIApplication.SharedApplication.KeyWindow.RootViewController,
                MobileServiceAuthenticationProvider.Facebook);

            if (user != null)
            {
                await Task.Delay(100);
                return true;
            }

            return false;
        }
    }
}
