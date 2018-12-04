using System.Threading.Tasks;
using Autofac;
using Poc.Mobile.App.Services.Interfaces;
using Poc.Mobile.App.Utilities;
using Poc.Mobile.App.ViewModels;
using Poc.Mobile.App.ViewModels.Connections;
using Poc.Mobile.App.Views;
using Poc.Mobile.App.Views.Connections;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MainPage = Poc.Mobile.App.Views.MainPage;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Poc.Mobile.App
{
	public partial class App : Application
	{
	    public new static App Current => Application.Current as App;
	    public Palette Colors;

        private readonly INavigationService _navigationService;
	    private readonly IAgentContextService _contextService;

        public App (IContainer container)
		{
			InitializeComponent();

            Colors.Init();

	        _navigationService = container.Resolve<INavigationService>();
		    _contextService = container.Resolve<IAgentContextService>();

		    InitializeTask = Initialize();
        }

	    public Task InitializeTask;
        private async Task Initialize()
	    {
	        //Pages
            _navigationService.AddPageViewModelBinding<MainViewModel, MainPage>();
	        _navigationService.AddPageViewModelBinding<ConnectionsViewModel, ConnectionsPage>();
            _navigationService.AddPageViewModelBinding<RegisterViewModel, RegisterPage>();

	        if (_contextService.AgentExists())
	            await _navigationService.NavigateToAsync<MainViewModel>();
	        else
	            await _navigationService.NavigateToAsync<RegisterViewModel>();
	    }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
