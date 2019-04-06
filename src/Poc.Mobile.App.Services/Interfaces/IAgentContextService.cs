using System.Threading.Tasks;
using AgentFramework.Core.Contracts;
using Poc.Mobile.App.Services.Models;

namespace Poc.Mobile.App.Services.Interfaces
{
    public interface ICustomAgentContextProvider : IAgentContextProvider
    {
        bool AgentExists();

        Task<bool> CreateAgentAsync(AgentOptions options);
    }
}
