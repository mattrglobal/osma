using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Poc.Mobile.App.Services.Interfaces;
using Poc.Mobile.App.Services.Models;
using Streetcred.Sdk.Models.Wallets;
using Xamarin.Forms;

namespace Poc.Mobile.App.ViewModels
{
    public class RegisterViewModel : ABaseViewModel
    {
        private readonly IAgentContextService _agentContextService;

        public RegisterViewModel(IUserDialogs userDialogs, 
                                 INavigationService navigationService,
                                 IAgentContextService agentContextService) : base(
                                 nameof(RegisterViewModel), 
                                 userDialogs, 
                                 navigationService)
        {
            _agentContextService = agentContextService;
        }

        #region Bindable Commands
        public ICommand CreateWalletCommand => new Command(async () =>
        {
            var dialog = UserDialogs.Instance.Loading("Creating wallet");
            
            //TODO this register VM will have far more logic around the registration complexities, i.e backupservices
            //suppling ownership info to the agent etc..
            var options = new AgentOptions
            {
                PoolOptions = new PoolOptions
                {
                    GenesisFilename = "pool_genesis.txn",
                    PoolName = "EdgeAgentPoolConnection",
                    ProtocolVersion = 2
                },
                WalletOptions = new WalletOptions
                {
                    WalletConfiguration = new WalletConfiguration {Id = Guid.NewGuid().ToString() },
                    WalletCredentials = new WalletCredentials {Key = "LocalWalletKey" }
                },
                EndpointUri = "http://mockagency.com"
            };

            if (await _agentContextService.CreateAgentAsync(options))
            {
                await NavigationService.NavigateToAsync<MainViewModel>();
                dialog?.Hide();
                dialog?.Dispose();
            }
            else
            {
                dialog?.Hide();
                dialog?.Dispose();
                UserDialogs.Instance.Alert("Failed to create wallet!");
            }
        });
        #endregion
    }
}
