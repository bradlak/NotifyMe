using NotifyMe.App.Infrastructure;
using Xamarin.Forms;
using NotifyMe.App.Droid.PlatformSpecific;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

[assembly: Dependency(typeof(DroidLoginService))]
namespace NotifyMe.App.Droid.PlatformSpecific
{
    public class DroidLoginService : ILoginService
    {
        private MobileServiceClient client;

        public DroidLoginService()
        {
            client = MobileServiceClientWrapper.Instance.Client;
        }

        public async Task<bool> Login()
        {
            var user = await client.LoginAsync(MainActivity.Instance, MobileServiceAuthenticationProvider.Facebook);

            if (user != null)
            {
                await Task.Delay(100);
                return true;
            }

            return false;
        }
    }
}