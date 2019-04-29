using System.Threading.Tasks;
using AgentFramework.Core.Contracts;
using Osma.Mobile.App.Services.Models;

namespace Osma.Mobile.App.Services.Interfaces
{
    public interface ICustomAgentContextProvider : IAgentContextProvider
    {
        bool AgentExists();

        Task<bool> CreateAgentAsync(AgentOptions options);
    }
}
