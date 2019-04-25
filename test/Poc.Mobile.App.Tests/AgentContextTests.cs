using Poc.Mobile.App.Services;
using System.Reflection;
using Xunit;

namespace Poc.Mobile.App.Tests
{
    public class AgentContextTests
    {
        private AgentContext context = new AgentContext
            {
                Did = "PNQm3CwyXbN5e39Rw3dXYx",
                Verkey = "~AHtGeRXtGjVfXALtXP9WiX"
                //Connection = "",
                //Wallet = "",
            };

        [Fact]
        public void AgentContextHasProperties()
        {
            Assert.True( context.GetType().GetTypeInfo().GetRuntimeProperty("Pool") != null );
            Assert.True( context.GetType().GetTypeInfo().GetRuntimeProperty("State") != null );
            Assert.True( context.GetType().GetTypeInfo().GetRuntimeProperty("Connection") != null );
            Assert.True( context.GetType().GetTypeInfo().GetRuntimeProperty("Wallet") != null );
            Assert.True( context.GetType().GetTypeInfo().GetRuntimeProperty("Did") != null );
            Assert.True( context.GetType().GetTypeInfo().GetRuntimeProperty("Verkey") != null );
        }


    }
}
