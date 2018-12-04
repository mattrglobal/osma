using System.Threading.Tasks;
using Poc.Mobile.App.Services.Models;

namespace Poc.Mobile.App.Services.Interfaces
{
    public interface IAgentContextService
    {
        bool AgentExists();

        Task<bool> CreateAgentAsync(AgentOptions options);

        Task<AgentContext> GetContextAsync();
    }
}
