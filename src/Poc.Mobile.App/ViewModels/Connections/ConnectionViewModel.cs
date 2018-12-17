using Acr.UserDialogs;
using Poc.Mobile.App.Services.Interfaces;
using ReactiveUI;
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

            ConnectionName = _record.Alias.Name;
            ConnectionSubtitle = $"{_record.State:G}";
            ConnectionImageUrl = _record.Alias.ImageUrl;
        }

        #region Bindable Properties
        private string _connectionName;
        public string ConnectionName
        {
            get => _connectionName;
            set => this.RaiseAndSetIfChanged(ref _connectionName, value);
        }

        private string _connectionImageUrl;
        public string ConnectionImageUrl
        {
            get => _connectionImageUrl;
            set => this.RaiseAndSetIfChanged(ref _connectionImageUrl, value);
        }

        private string _connectionSubtitle = "Lorem ipsum dolor sit amet";
        public string ConnectionSubtitle
        {
            get => _connectionSubtitle;
            set => this.RaiseAndSetIfChanged(ref _connectionSubtitle, value);
        }
        #endregion
    }
}
