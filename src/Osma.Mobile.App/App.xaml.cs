using System.Threading.Tasks;
using Autofac;
using Osma.Mobile.App.Services.Interfaces;
using Osma.Mobile.App.Utilities;
using Osma.Mobile.App.ViewModels;
using Osma.Mobile.App.ViewModels.Account;
using Osma.Mobile.App.ViewModels.Connections;
using Osma.Mobile.App.ViewModels.Credentials;
using Osma.Mobile.App.Views;
using Osma.Mobile.App.Views.Account;
using Osma.Mobile.App.Views.Connections;
using Osma.Mobile.App.Views.Credentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using MainPage = Osma.Mobile.App.Views.MainPage;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Osma.Mobile.App
{
    public partial class App : Application
    {
        public new static App Current => Application.Current as App;
        public Palette Colors;

        private readonly INavigationService _navigationService;
        private readonly ICustomAgentContextProvider _contextProvider;

        public App(IContainer container)
        {
            InitializeComponent();

            Colors.Init();
            _navigationService = container.Resolve<INavigationService>();
            _contextProvider = container.Resolve<ICustomAgentContextProvider>();

            InitializeTask = Initialize();
        }

        Task InitializeTask;
        private async Task Initialize()
        {
            //Pages
            _navigationService.AddPageViewModelBinding<MainViewModel, MainPage>();
            _navigationService.AddPageViewModelBinding<ConnectionsViewModel, ConnectionsPage>();
            _navigationService.AddPageViewModelBinding<ConnectionViewModel, ConnectionPage>();
            _navigationService.AddPageViewModelBinding<RegisterViewModel, RegisterPage>();
            _navigationService.AddPageViewModelBinding<AcceptInviteViewModel, AcceptInvitePage>();
            _navigationService.AddPageViewModelBinding<CredentialsViewModel, CredentialsPage>();
            _navigationService.AddPageViewModelBinding<CredentialViewModel, CredentialPage>();
            _navigationService.AddPageViewModelBinding<AccountViewModel, AccountPage>();

            if (_contextProvider.AgentExists())
            {
                await _navigationService.NavigateToAsync<MainViewModel>();
            }
            else
            {
                await _navigationService.NavigateToAsync<RegisterViewModel>();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
