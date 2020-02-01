using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Storage;
using Osma.Mobile.App.Services.Interfaces;
using Xamarin.Forms;

namespace Osma.Mobile.App.ViewModels
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

                GenesisFilename = "pool_genesis.txn",
                PoolName = "EdgeAgentPoolConnection",
                ProtocolVersion = 2,
                
                WalletConfiguration = new WalletConfiguration {Id = Guid.NewGuid().ToString() },
                WalletCredentials = new WalletCredentials {Key = "LocalWalletKey" }
                
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
