using Hyperledger.Indy.PoolApi;
using Hyperledger.Indy.WalletApi;

namespace Poc.Mobile.App.Services
{
    /// <summary>
    /// Agent context.
    /// </summary>
    public class AgentContext
    {
        /// <summary>
        /// Gets or sets the agent did.
        /// </summary>
        /// <value>The agent did.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the pool.
        /// </summary>
        /// <value>The pool.</value>
        public Pool Pool { get; set; }

        /// <summary>
        /// Gets or sets the wallet.
        /// </summary>
        /// <value>The wallet.</value>
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
