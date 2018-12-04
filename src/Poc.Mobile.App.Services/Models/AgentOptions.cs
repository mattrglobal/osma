namespace Poc.Mobile.App.Services.Models
{
    /// <summary>
    ///     Agent options.
    /// </summary>
    public class AgentOptions
    {
        /// <summary>
        ///     Gets or sets the agent name.
        /// </summary>
        /// <value>The agent name.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the wallet configuration.
        /// </summary>
        /// <value>The wallet configuration.</value>
        public WalletOptions WalletOptions { get; set; }

        /// <summary>
        ///     Gets or sets the pool options.
        /// </summary>
        /// <value>The pool options.</value>
        public PoolOptions PoolOptions { get; set; }

        public string EndpointUri { get; set; } = "127.0.0.1:5000";

        /// <summary>
        ///     Gets or sets the agent did.
        /// </summary>
        /// <value>The agent did.</value>
        public string Did { get; set; }

        /// <summary>
        ///     Gets or sets the seed.
        /// </summary>
        /// <value>The seed.</value>
        public string Seed { get; set; }

        /// <summary>
        ///     Gets or sets the agent identifier.
        /// </summary>
        /// <value>The agent identifier.</value>
        public string AgentId { get; set; }
    }
}
