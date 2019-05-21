using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using AgentFramework.Core.Contracts;
using AgentFramework.Core.Messages;
using AgentFramework.Core.Messages.Common;
using AgentFramework.Core.Messages.Discovery;
using AgentFramework.Core.Models.Records;
using Osma.Mobile.App.Events;
using Osma.Mobile.App.Extensions;
using Osma.Mobile.App.Services.Interfaces;
using Osma.Mobile.App.Views.Connections;
using ReactiveUI;
using Xamarin.Forms;

namespace Osma.Mobile.App.ViewModels.Connections
{
    public class ConnectionViewModel : ABaseViewModel
    {
        private readonly ConnectionRecord _record;

        private readonly IAgentContextProvider _agentContextProvider;
        private readonly IMessageService _messageService;
        private readonly IDiscoveryService _discoveryService;
        private readonly IConnectionService _connectionService;
        private readonly IEventAggregator _eventAggregator;

        public ConnectionViewModel(IUserDialogs userDialogs,
                                   INavigationService navigationService,
                                   IAgentContextProvider agentContextProvider,
                                   IMessageService messageService,
                                   IDiscoveryService discoveryService,
                                   IConnectionService connectionService,
                                   IEventAggregator eventAggregator,
                                   ConnectionRecord record) :
                                   base(nameof(ConnectionViewModel),
                                       userDialogs,
                                       navigationService)
        {
            _agentContextProvider = agentContextProvider;
            _messageService = messageService;
            _discoveryService = discoveryService;
            _connectionService = connectionService;
            _eventAggregator = eventAggregator;

            _record = record;
            MyDid = _record.MyDid;
            TheirDid = _record.TheirDid;
            ConnectionName = _record.Alias.Name;
            ConnectionSubtitle = $"{_record.State:G}";
            ConnectionImageUrl = _record.Alias.ImageUrl;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await RefreshTransactions();
            await base.InitializeAsync(navigationData);
        }

        public async Task RefreshTransactions()
        {
            RefreshingTransactions = true;

            var context = await _agentContextProvider.GetContextAsync();
            var message = _discoveryService.CreateQuery(context, "*");

            DiscoveryDiscloseMessage protocols = null;

            try
            {
                var response = await _messageService.SendAsync(context.Wallet, message, _record, null, true);
                protocols = response.GetMessage<DiscoveryDiscloseMessage>();
            }
            catch (Exception)
            {
                //Swallow exception
                //TODO more granular error protection
            }

            IList<TransactionItem> transactions = new List<TransactionItem>();

            if (protocols != null)
            {
                foreach (var protocol in protocols.Protocols)
                {
                    switch (protocol.ProtocolId)
                    {
                        case MessageTypes.TrustPingMessageType:
                            transactions.Add(new TransactionItem()
                            {
                                Title = "Trust Ping",
                                Subtitle = "Version 1.0",
                                PrimaryActionTitle = "Ping",
                                PrimaryActionCommand = new Command(async () =>
                                {
                                    await PingConnectionAsync();
                                }, () => true),
                                Type = TransactionItemType.Action.ToString("G")
                            });
                            break;
                    }
                }
            }

            Transactions.Clear();
            Transactions.InsertRange(transactions);
            HasTransactions = transactions.Any();

            RefreshingTransactions = false;
        }

        public async Task PingConnectionAsync()
        {
            var dialog = UserDialogs.Instance.Loading("Pinging");
            var context = await _agentContextProvider.GetContextAsync();
            var message = new TrustPingMessage
            {
                ResponseRequested = true
            };

            bool success = false;
            try
            {
                var rsp = await _messageService.SendAsync(context.Wallet, message, _record, null, true);
                var trustPingResponse = rsp.GetMessage<TrustPingResponseMessage>();
                success = true;
            }
            catch (Exception)
            {
                //Swallow exception
                //TODO more granular error protection
            }

            if (dialog.IsShowing)
            {
                dialog.Hide();
                dialog.Dispose();
            }

            DialogService.Alert(
                    success ?
                    "Ping Response Recieved" :
                    "No Ping Response Recieved"
                );
        }

        #region Bindable Command
        public ICommand NavigateBackCommand => new Command(async () =>
        {
            await NavigationService.PopModalAsync();
        });

        public ICommand DeleteConnectionCommand => new Command(async () =>
        {
            var dialog = DialogService.Loading("Deleting");

            var context = await _agentContextProvider.GetContextAsync();
            await _connectionService.DeleteAsync(context, _record.Id);

            _eventAggregator.Publish(new ApplicationEvent() { Type = ApplicationEventType.ConnectionsUpdated });

            if (dialog.IsShowing)
            {
                dialog.Hide();
                dialog.Dispose();
            }

            await NavigationService.PopModalAsync();
        });

        public ICommand RefreshTransactionsCommand => new Command(async () => await RefreshTransactions());
        #endregion

        #region Bindable Properties
        private string _connectionName;
        public string ConnectionName
        {
            get => _connectionName;
            set => this.RaiseAndSetIfChanged(ref _connectionName, value);
        }

        private string _myDid;
        public string MyDid
        {
            get => _myDid;
            set => this.RaiseAndSetIfChanged(ref _myDid, value);
        }

        private string _theirDid;
        public string TheirDid
        {
            get => _theirDid;
            set => this.RaiseAndSetIfChanged(ref _theirDid, value);
        }

        private string _connectionImageUrl;
        public string ConnectionImageUrl
        {
            get => _connectionImageUrl;
            set => this.RaiseAndSetIfChanged(ref _connectionImageUrl, value);
        }

        private string _connectionSubtitle = "Lorem ipsum dolor sit amet";
        public string ConnectionSubtitle
        {
            get => _connectionSubtitle;
            set => this.RaiseAndSetIfChanged(ref _connectionSubtitle, value);
        }

        private RangeEnabledObservableCollection<TransactionItem> _transactions = new RangeEnabledObservableCollection<TransactionItem>();
        public RangeEnabledObservableCollection<TransactionItem> Transactions
        {
            get => _transactions;
            set => this.RaiseAndSetIfChanged(ref _transactions, value);
        }

        private bool _refreshingTransactions;
        public bool RefreshingTransactions
        {
            get => _refreshingTransactions;
            set => this.RaiseAndSetIfChanged(ref _refreshingTransactions, value);
        }

        private bool _hasTransactions;
        public bool HasTransactions
        {
            get => _hasTransactions;
            set => this.RaiseAndSetIfChanged(ref _hasTransactions, value);
        }
        #endregion
    }
}
