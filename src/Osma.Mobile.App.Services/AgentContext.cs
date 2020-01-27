using System.Collections.Generic;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Ledger;
using Hyperledger.Indy.WalletApi;

namespace Osma.Mobile.App.Services
{
    /// <inheritdoc />
    public class AgentContext : IAgentContext
    {
        /// <inheritdoc />
        public PoolAwaitable Pool { get; set; }

        /// <inheritdoc />
        public Dictionary<string, string> State { get; set; }

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

        /// <inheritdoc />
        public IList<MessageType> SupportedMessages { get; set; } = new List<MessageType>();
        public IAgent Agent { get; set; }
    }
}
