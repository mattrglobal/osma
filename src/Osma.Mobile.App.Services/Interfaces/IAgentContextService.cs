using System.Threading.Tasks;
using AgentFramework.Core.Handlers.Agents;
using Osma.Mobile.App.Services.Models;

namespace Osma.Mobile.App.Services.Interfaces
{
    public interface ICustomAgentContextProvider : IAgentProvider
    {
        bool AgentExists();

        Task<bool> CreateAgentAsync(AgentOptions options);
    }
}
