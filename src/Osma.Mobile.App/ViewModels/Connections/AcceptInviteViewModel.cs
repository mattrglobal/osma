using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using AgentFramework.Core.Contracts;
using AgentFramework.Core.Messages.Connections;
using Osma.Mobile.App.Services.Interfaces;
using ReactiveUI;
using Xamarin.Forms;

namespace Osma.Mobile.App.ViewModels.Connections
{
    public class AcceptInviteViewModel : ABaseViewModel
    {
        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly IAgentContextProvider _contextProvider;
        private const string Contents = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua";

        private ConnectionInvitationMessage _invite;

        public AcceptInviteViewModel(IUserDialogs userDialogs,
                                     INavigationService navigationService,
                                     IConnectionService connectionService,
                                     IMessageService messageService,
                                     IAgentContextProvider contextProvider)
                                     : base("Accept Invitiation", userDialogs, navigationService)
        {
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _messageService = messageService;
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is ConnectionInvitationMessage invite)
            {
                InviteTitle = $"Connect with {invite.Label}?";
                InviterUrl = invite.ImageUrl;
                InviteContents = $"{invite.Label} would like to establish a pairwise DID connection with you. This will allow secure communication between you and {invite.Label}.";
                _invite = invite;
            }
            return base.InitializeAsync(navigationData);
        }

        #region Bindable Commands
        public ICommand AcceptInviteCommand => new Command(async () =>
        {
            var loadingDialog = DialogService.Loading("Processing");

            var context = await _contextProvider.GetContextAsync();

            if (context == null || _invite == null)
            {
                loadingDialog.Hide();
                DialogService.Alert("Failed to accept invite!");
                return;
            }

            try
            {
                var (msg, rec) = await _connectionService.CreateRequestAsync(context, _invite);
                await _messageService.SendToConnectionAsync(context.Wallet, msg, rec, _invite.RecipientKeys.First());
            }
            catch (Exception) //TODO more granular error protection
            {
                loadingDialog.Hide();
                DialogService.Alert("Failed to accept invite!");
            }

            if (loadingDialog.IsShowing)
                loadingDialog.Hide();

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

        private string _inviteContents = Contents;
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
