using System;
using System.Threading.Tasks;
using AgentFramework.Core.Contracts;
using AgentFramework.Core.Models.Wallets;
using Poc.Mobile.App.Services.Interfaces;
using Poc.Mobile.App.Services.Models;

namespace Poc.Mobile.App.Services
{
    public class AgentContextService : IAgentContextService
    {
        private readonly IWalletService _walletService;
        private readonly IPoolService _poolService;
        private readonly IProvisioningService _provisioningService;
        private readonly IKeyValueStoreService _keyValueStoreService;

        private const string AgentOptionsKey = "AgentOptions";

        private AgentOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:POC.Services.Runtime.ContextService" /> class.
        /// </summary>
        /// <param name="walletService">Wallet service.</param>
        /// <param name="poolService">The pool service.</param>
        /// <param name="provisioningService">The provisioning service.</param>
        /// <param name="keyValueStoreService">Key value store.</param>
        public AgentContextService(IWalletService walletService,
            IPoolService poolService,
            IProvisioningService provisioningService,
            IKeyValueStoreService keyValueStoreService)
        {
            _poolService = poolService;
            _provisioningService = provisioningService;
            _walletService = walletService;
            _keyValueStoreService = keyValueStoreService;

            if (_keyValueStoreService.KeyExists(AgentOptionsKey))
                _options = _keyValueStoreService.GetData<AgentOptions>(AgentOptionsKey);
        }
        
        public async Task<bool> CreateAgentAsync(AgentOptions options)
        {
            await _walletService.CreateWalletAsync(options.WalletOptions.WalletConfiguration, options.WalletOptions.WalletCredentials);

            var wallet = await _walletService.GetWalletAsync(options.WalletOptions.WalletConfiguration, options.WalletOptions.WalletCredentials);

            await _provisioningService.ProvisionAgentAsync(wallet, new ProvisioningConfiguration
            {
                AgentSeed = options.Seed,
                EndpointUri = new Uri($"{options.EndpointUri}"),
                TailsBaseUri = new Uri($"{options.EndpointUri}/tails") //TODO SDK-issue this Uri is required even when creating a non-issuer agent, throws generic NRE when not present.
            });

            await _keyValueStoreService.SetDataAsync(AgentOptionsKey, options);
            _options = options;

            return true;
        }

        /// <summary>
        /// Returns the agent context containing wallet and agent information
        /// </summary>
        /// <returns></returns>
        public virtual async Task<AgentContext> GetContextAsync()
        {
            if(!AgentExists())//TODO uniform approach to error protection
                throw new Exception("Agent doesnt exist");

            var wallet = await _walletService.GetWalletAsync(_options.WalletOptions.WalletConfiguration, _options.WalletOptions.WalletCredentials);

            return new AgentContext
            {
                Did = _options.Did,
                Id = _options.AgentId,
                Wallet = wallet
            };
        }

        public bool AgentExists() => _options != null;
    }
}
