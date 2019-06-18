using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using AgentFramework.Core.Contracts;
using AgentFramework.Core.Messages.Connections;
using AgentFramework.Core.Exceptions;
using Osma.Mobile.App.Events;
using Osma.Mobile.App.Services.Interfaces;
using ReactiveUI;
using Xamarin.Forms;

namespace Osma.Mobile.App.ViewModels.Connections
{
    public class AcceptInviteViewModel : ABaseViewModel
    {
        private readonly IProvisioningService _provisioningService;
        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly IAgentContextProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;
        private static readonly String GENERIC_CONNECTION_REQUEST_FAILURE_MESSAGE = "Failed to accept invite!";

        private ConnectionInvitationMessage _invite;

        public AcceptInviteViewModel(IUserDialogs userDialogs,
                                     INavigationService navigationService,
                                     IProvisioningService provisioningService,
                                     IConnectionService connectionService,
                                     IMessageService messageService,
                                     IAgentContextProvider contextProvider,
                                     IEventAggregator eventAggregator)
                                     : base("Accept Invitiation", userDialogs, navigationService)
        {
            _provisioningService = provisioningService;
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _messageService = messageService;
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is ConnectionInvitationMessage invite)
            {
                InviteTitle = $"Trust {invite.Label}?";
                InviterUrl = invite.ImageUrl;
                InviteContents = $"{invite.Label} would like to establish a pairwise DID connection with you. This will allow secure communication between you and {invite.Label}.";
                _invite = invite;
            }
            return base.InitializeAsync(navigationData);
        }

        private async Task<String> CreateConnection(IAgentContext context, ConnectionInvitationMessage invite)
        {
            try
            {
                var provisioningRecord = await _provisioningService.GetProvisioningAsync(context.Wallet);

                if (provisioningRecord.Endpoint.Uri != null)
                {
                    try
                    {
                        var (msg, rec) = await _connectionService.CreateRequestAsync(context, _invite);
                        await _messageService.SendAsync(context.Wallet, msg, rec, _invite.RecipientKeys.First());
                        return String.Empty;
                    }
                    catch (Exception) //TODO more granular error protection
                    {
                        return GENERIC_CONNECTION_REQUEST_FAILURE_MESSAGE;
                    }
                }
                else
                {
                    try
                    {
                        var (msg, rec) = await _connectionService.CreateRequestAsync(context, _invite);
                        var rsp = await _messageService.SendAsync(context.Wallet, msg, rec, _invite.RecipientKeys.First(), true);
                        await _connectionService.ProcessResponseAsync(context, rsp.GetMessage<ConnectionResponseMessage>(), rec);
                        return String.Empty;
                    }
                    catch (Exception) //TODO more granular error protection
                    {
                        return GENERIC_CONNECTION_REQUEST_FAILURE_MESSAGE;
                    }
                }
            } catch(AgentFrameworkException agentFrameworkException)
            {
                return agentFrameworkException.Message;
            }
        }

        #region Bindable Commands
        public ICommand AcceptInviteCommand => new Command(async () =>
        {
            var loadingDialog = DialogService.Loading("Processing");

            var context = await _contextProvider.GetContextAsync();

            if (context == null || _invite == null)
            {
                loadingDialog.Hide();
                DialogService.Alert("Failed to decode invite!");
                return;
            }

            var result = await CreateConnection(context, _invite);

            _eventAggregator.Publish(new ApplicationEvent() { Type = ApplicationEventType.ConnectionsUpdated });

            if (loadingDialog.IsShowing)
                loadingDialog.Hide();

            if (!String.IsNullOrEmpty(result))
                DialogService.Alert(result);

            await NavigationService.PopModalAsync();
        });

        public ICommand RejectInviteCommand => new Command(async () => await NavigationService.PopModalAsync());

        #endregion

        #region Bindable Properties
        private string _inviteTitle;
        public string InviteTitle
        {
            get => _inviteTitle;
            set => this.RaiseAndSetIfChanged(ref _inviteTitle, value);
        }

        private string _inviteContents = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua";
        public string InviteContents
        {
            get => _inviteContents;
            set => this.RaiseAndSetIfChanged(ref _inviteContents, value);
        }

        private string _inviterUrl;
        public string InviterUrl
        {
            get => _inviterUrl;
            set => this.RaiseAndSetIfChanged(ref _inviterUrl, value);
        }
        #endregion
    }
}
