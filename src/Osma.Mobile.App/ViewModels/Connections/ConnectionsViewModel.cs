using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using AgentFramework.Core.Contracts;
using AgentFramework.Core.Messages.Connections;
using AgentFramework.Core.Utils;
using Autofac;
using Osma.Mobile.App.Extensions;
using Osma.Mobile.App.Services;
using Osma.Mobile.App.Services.Interfaces;
using ReactiveUI;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Osma.Mobile.App.ViewModels.Connections
{
    public class ConnectionsViewModel : ABaseViewModel
    {
        private readonly IConnectionService _connectionService;
        private readonly ICustomAgentContextProvider _agentContextProvider;
        private readonly ILifetimeScope _scope;

        public ConnectionsViewModel(IUserDialogs userDialogs,
                                    INavigationService navigationService,
                                    IConnectionService connectionService,
                                    ICustomAgentContextProvider agentContextProvider,
                                    ILifetimeScope scope) :
                                    base("My Connections", userDialogs, navigationService)
        {
            _connectionService = connectionService;
            _agentContextProvider = agentContextProvider;
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

            var context = await _agentContextProvider.GetContextAsync();
            var records = await _connectionService.ListAsync(context);

            #if DEBUG
            var exampleRecord = new AgentFramework.Core.Models.Records.ConnectionRecord
            {
                Id = Guid.NewGuid().ToString().ToLowerInvariant(),
                Alias = new AgentFramework.Core.Models.Connections.ConnectionAlias {
                    Name = "Example Connection",
                    ImageUrl = "https://placehold.it/300x300"
                },
                MyDid = "sov:7N2DqXEPRG7wbqJvJL3diU",
                State = AgentFramework.Core.Models.Records.ConnectionState.Connected,
                TheirDid = "sov:KNWvuaPtWtL8fgaArBeKr1",
            };
            records.Add(exampleRecord);
            #endif

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

        public async Task ScanInvite()
        {
            var expectedFormat = ZXing.BarcodeFormat.QR_CODE;
            var opts = new ZXing.Mobile.MobileBarcodeScanningOptions{ PossibleFormats = new List<ZXing.BarcodeFormat> { expectedFormat }};

            var scannerPage = new ZXingScannerPage(opts);
            scannerPage.OnScanResult += (result) => {
                scannerPage.IsScanning = false;

                ConnectionInvitationMessage invitation;

                try
                {
                    invitation = MessageUtils.DecodeMessageFromUrlFormat<ConnectionInvitationMessage>(result.Text);
                }
                catch (Exception)
                {
                    DialogService.Alert("Invalid invitation!");
                    Device.BeginInvokeOnMainThread(async () => await NavigationService.PopModalAsync());
                    return;
                }

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await NavigationService.PopModalAsync();
                    await NavigationService.NavigateToAsync<AcceptInviteViewModel>(invitation, NavigationType.Modal);
                });
            };

            await NavigationService.NavigateToAsync((Page)scannerPage, NavigationType.Modal);
        }

        public async Task SelectConnection(ConnectionViewModel connection) => await NavigationService.NavigateToAsync(connection, null, NavigationType.Modal);

        #region Bindable Command
        public ICommand RefreshCommand => new Command(async () => await RefreshConnections());

        public ICommand ScanInviteCommand => new Command(async () => await ScanInvite());

        public ICommand SelectConnectionCommand => new Command<ConnectionViewModel>(async (connection) =>
        {
            if (connection != null)
                await SelectConnection(connection);
        });
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
