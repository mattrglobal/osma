using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Autofac;
using Poc.Mobile.App.Extensions;
using Poc.Mobile.App.Services.Interfaces;
using ReactiveUI;
using Streetcred.Sdk.Contracts;
using Xamarin.Forms;

namespace Poc.Mobile.App.ViewModels.Connections
{
    public class ConnectionsViewModel : ABaseViewModel
    {
        private readonly IConnectionService _connectionService;
        private readonly IAgentContextService _agentContextService;
        private readonly ILifetimeScope _scope;

        public ConnectionsViewModel(IUserDialogs userDialogs,
                                    INavigationService navigationService,
                                    IConnectionService connectionService,
                                    IAgentContextService agentContextService,
                                    ILifetimeScope scope) :
                                    base("My Connections", userDialogs, navigationService)
        {
            _connectionService = connectionService;
            _agentContextService = agentContextService;
            _scope = scope;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await RefreshConnections();
            await base.InitializeAsync(navigationData);
        }

        public async Task RefreshConnections()
        {
            RefreshingConnections = true;

            var context = await _agentContextService.GetContextAsync();

            var records = await _connectionService.ListAsync(context.Wallet);

            IList<ConnectionViewModel> connectionVms = new List<ConnectionViewModel>();
            foreach (var record in records)
            {
                var connection = _scope.Resolve<ConnectionViewModel>(new NamedParameter("record", record));
                connectionVms.Add(connection);
            }

            //TODO need to compare with the currently displayed connections rather than disposing all of them
            Connections.Clear();
            Connections.InsertRange(connectionVms);
            HasConnections = connectionVms.Any();

            RefreshingConnections = false;
        }

        #region Bindable Command
        public ICommand RefreshCommand => new Command(async () => await RefreshConnections());
        #endregion

        #region Bindable Properties
        private RangeEnabledObservableCollection<ConnectionViewModel> _connections = new RangeEnabledObservableCollection<ConnectionViewModel>();
        public RangeEnabledObservableCollection<ConnectionViewModel> Connections
        {
            get => _connections;
            set => this.RaiseAndSetIfChanged(ref _connections, value);
        }

        private bool _hasConnections;
        public bool HasConnections
        {
            get => _hasConnections;
            set => this.RaiseAndSetIfChanged(ref _hasConnections, value);
        }

        private bool _refreshingConnections;
        public bool RefreshingConnections
        {
            get => _refreshingConnections;
            set => this.RaiseAndSetIfChanged(ref _refreshingConnections, value);
        }
        #endregion
    }
}
