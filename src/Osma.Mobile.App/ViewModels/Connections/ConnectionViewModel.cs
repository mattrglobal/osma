using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using AgentFramework.Core.Contracts;
using AgentFramework.Core.Models.Records;
using Osma.Mobile.App.Events.Osma.Mobile.App.Events;
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
        private readonly IConnectionService _connectionService;
        private readonly IEventAggregator _eventAggregator;

        public ConnectionViewModel(IUserDialogs userDialogs,
                                   INavigationService navigationService,
                                   IAgentContextProvider agentContextProvider,
                                   IConnectionService connectionService,
                                   IEventAggregator eventAggregator,
                                   ConnectionRecord record) :
                                   base(nameof(ConnectionViewModel),
                                       userDialogs,
                                       navigationService)
        {
            _agentContextProvider = agentContextProvider;
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

        public Task RefreshTransactions()
        {
            RefreshingTransactions = true;

            // TODO OS-201 Get the transactions
            IList<TransactionItem> transactions = new List<TransactionItem>();

            #if DEBUG
            transactions.Add(new TransactionItem
            {
                Title = "test",
                Subtitle = "test subtitle",
                Datetime = "YYYY-MM-DD",
                Type = TransactionItemType.Actionable.ToString("G")
            });
            transactions.Add(new TransactionItem
            {
                Title = "test",
                Subtitle = "test subtitle",
                Datetime = "YYYY-MM-DD",
                Type = TransactionItemType.Status.ToString("G")
            });
            #endif

            Transactions.Clear();
            Transactions.InsertRange(transactions);
            HasTransactions = transactions.Any();

            RefreshingTransactions = false;

            return Task.FromResult(true);
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

            _eventAggregator.Publish(new ApplicationEvent() { EventType = ApplicationEventType.ConnectionsUpdated });

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
