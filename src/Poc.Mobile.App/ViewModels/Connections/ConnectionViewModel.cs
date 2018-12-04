using Acr.UserDialogs;
using Poc.Mobile.App.Services.Interfaces;
using Streetcred.Sdk.Models.Records;

namespace Poc.Mobile.App.ViewModels.Connections
{
    public class ConnectionViewModel : ABaseViewModel
    {
        private readonly ConnectionRecord _record;

        public ConnectionViewModel(IUserDialogs userDialogs,
                                   INavigationService navigationService,
                                   ConnectionRecord record) :
                                   base(nameof(ConnectionViewModel),
                                       userDialogs,
                                       navigationService)
        {
            _record = record;
        }
    }
}
