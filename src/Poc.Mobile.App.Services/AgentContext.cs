using System.Collections.Generic;
using AgentFramework.Core.Contracts;
using AgentFramework.Core.Models;
using AgentFramework.Core.Models.Records;
using Hyperledger.Indy.PoolApi;
using Hyperledger.Indy.WalletApi;

namespace Poc.Mobile.App.Services
{
    /// <inheritdoc />
    public class AgentContext : IAgentContext
    {
        /// <inheritdoc />
        public PoolAwaitable Pool { get; set; }

        /// <inheritdoc />
        public Dictionary<string, string> State { get; set; }

        /// <inheritdoc />
        public ConnectionRecord Connection { get; set; }

        /// <inheritdoc />
        public Wallet Wallet { get; set; }
        
        /// <summary>
        /// Gets or sets the agent did.
        /// </summary>
        /// <value>The agent did.</value>
        public string Did { get; set; }

        /// <summary>
        /// Gets or sets the agent verkey.
        /// </summary>
        /// <value>The agent verkey.</value>
        public string Verkey { get; set; }
    }
}
