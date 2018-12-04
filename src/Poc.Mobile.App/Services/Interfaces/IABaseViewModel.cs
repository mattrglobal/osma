using System.Threading.Tasks;

namespace Poc.Mobile.App.Services.Interfaces
{
    public interface IABaseViewModel
    {
        string Name { get; set; }

        string Title { get; set; }

        bool IsBusy { get; set; }

        Task InitializeAsync(object navigationData);
    }
}
