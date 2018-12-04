using System.Threading.Tasks;
using Acr.UserDialogs;
using Poc.Mobile.App.Services.Interfaces;
using Poc.Mobile.App.ViewModels.Connections;
using ReactiveUI;

namespace Poc.Mobile.App.ViewModels
{
    public class MainViewModel : ABaseViewModel
    {
        public MainViewModel(IUserDialogs userDialogs,
                             INavigationService navigationService,
                             ConnectionsViewModel connectionsViewModel) :
                             base(nameof(MainViewModel), userDialogs, navigationService)
        {
            Connections = connectionsViewModel;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await Connections.InitializeAsync(null);
            await base.InitializeAsync(navigationData);
        }

        #region Bindable Properties
        private ConnectionsViewModel _connections;
        public ConnectionsViewModel Connections
        {
            get => _connections;
            set => this.RaiseAndSetIfChanged(ref _connections, value);
        }
        #endregion
    }
}
