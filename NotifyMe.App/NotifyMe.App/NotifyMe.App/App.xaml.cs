using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using NotifyMe.App.Infrastructure;
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

        public static IAuthenticate Authenticator { get; private set; }

        public App()
        {
            InitializeComponent();

            Container = SimpleIoc.Default;

            var navigationService = new NavigationService();
            navigationService.Configure(ViewTypes.LoginPage, typeof(LoginPage));
            navigationService.Configure(ViewTypes.FriendsPage, typeof(FriendsPage));
            navigationService.Configure(ViewTypes.MessageCreate, typeof(CreateMessagePage));

            Container.Register<LoginViewModel>();
            Container.Register<FriendsViewModel>();
            Container.Register<CreateMessageViewModel>();
            Container.Register<INavigationService>(() => navigationService);
            Container.Register<IFacebookService, FacebookService>();

            Container.Register<LoginPage>();
            Container.Register<FriendsPage>();
            Container.Register<CreateMessagePage>();

            var mainPage = new NavigationPage(new LoginPage());
            navigationService.Initialize(mainPage);

            MainPage = mainPage;
        }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }
    }
}
