using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using AgentFramework.Core.Contracts;
using AgentFramework.Core.Messages.Connections;
using Poc.Mobile.App.Services.Interfaces;
using ReactiveUI;
using Xamarin.Forms;

namespace Poc.Mobile.App.ViewModels.Connections
{
    public class AcceptInviteViewModel : ABaseViewModel
    {
        private readonly IConnectionService _connectionService;
        private readonly IAgentContextService _contextService;

        private ConnectionInvitationMessage _invite;

        public AcceptInviteViewModel(IUserDialogs userDialogs,
                                     INavigationService navigationService,
                                     IConnectionService connectionService,
                                     IAgentContextService contextService)
                                     : base("Accept Invitiation", userDialogs, navigationService)
        {
            _connectionService = connectionService;
            _contextService = contextService;
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is ConnectionInvitationMessage invite)
            {
                InviteTitle = $"Trust {invite.Name}?";
                InviterUrl = invite.ImageUrl;
                _invite = invite;
            }
            return base.InitializeAsync(navigationData);
        }

        #region Bindable Commands
        public ICommand AcceptInviteCommand => new Command(async () =>
        {
            var loadingDialog = DialogService.Loading("Processing");

            var context = await _contextService.GetContextAsync();

            if (context == null || _invite == null)
            {
                loadingDialog.Hide();
                DialogService.Alert("Failed to accept invite!");
                return;
            }

            try
            {
                await _connectionService.AcceptInvitationAsync(context.Wallet, _invite);
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
