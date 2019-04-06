using System;
using System.Windows.Input;
using Acr.UserDialogs;
using AgentFramework.Core.Models.Wallets;
using Poc.Mobile.App.Services.Interfaces;
using Poc.Mobile.App.Services.Models;
using Xamarin.Forms;

namespace Poc.Mobile.App.ViewModels
{
    public class RegisterViewModel : ABaseViewModel
    {
        private readonly ICustomAgentContextProvider _agentContextProvider;

        public RegisterViewModel(IUserDialogs userDialogs, 
                                 INavigationService navigationService,
                                 ICustomAgentContextProvider agentContextProvider) : base(
                                 nameof(RegisterViewModel), 
                                 userDialogs, 
                                 navigationService)
        {
            _agentContextProvider = agentContextProvider;
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

            if (await _agentContextProvider.CreateAgentAsync(options))
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
