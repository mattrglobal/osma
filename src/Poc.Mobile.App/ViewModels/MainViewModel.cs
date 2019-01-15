using System.Threading.Tasks;
using Acr.UserDialogs;
using Poc.Mobile.App.Services.Interfaces;
using Poc.Mobile.App.ViewModels.Connections;
using Poc.Mobile.App.ViewModels.Credentials;
using ReactiveUI;

namespace Poc.Mobile.App.ViewModels
{
    public class MainViewModel : ABaseViewModel
    {
        public MainViewModel(
            IUserDialogs userDialogs,
            INavigationService navigationService,
            ConnectionsViewModel connectionsViewModel,
            CredentialsViewModel credentialsViewModel
        ) : base(
                nameof(MainViewModel),
                userDialogs,
                navigationService
        )
        {
            Connections = connectionsViewModel;
            Credentials = credentialsViewModel;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await Connections.InitializeAsync(null);
            await Credentials.InitializeAsync(null);
            await base.InitializeAsync(navigationData);
        }

        #region Bindable Properties
        private ConnectionsViewModel _connections;
        public ConnectionsViewModel Connections
        {
            get => _connections;
            set => this.RaiseAndSetIfChanged(ref _connections, value);
        }

        private CredentialsViewModel _credentials;
        public CredentialsViewModel Credentials
        {
            get => _credentials;
            set => this.RaiseAndSetIfChanged(ref _credentials, value);
        }
        #endregion
    }
}
