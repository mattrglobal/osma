using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Osma.Mobile.App.Services;
using Osma.Mobile.App.Services.Interfaces;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Essentials;
using Osma.Mobile.App.Views.Legal;

namespace Osma.Mobile.App.ViewModels.Account
{
    public class AccountViewModel : ABaseViewModel
    {
        public AccountViewModel(
            IUserDialogs userDialogs,
            INavigationService navigationService
        ) : base(
            "Account",
            userDialogs,
            navigationService
        )
        {
            _appVersion = AppInfo.VersionString;
            _buildVersion = AppInfo.BuildString;
#if DEBUG
            _fullName = "Jamie Doe";
            _avatarUrl = "http://i.pravatar.cc/100";
#endif
        }


        public async Task NavigateToBackup()
        {
            await DialogService.AlertAsync("Navigate to Backup");
        }

        public async Task NavigateToAuthentication()
        {
            await DialogService.AlertAsync("Navigate to authentication");
        }

        public async Task NavigateToLegalPage()
        {
            var legalPage = new LegalPage();
            await NavigationService.NavigateToAsync(legalPage, NavigationType.Modal);
        }

        public async Task NavigateToDebug()
        {
            await DialogService.AlertAsync("Navigate to debug page");
        }

        #region Bindable Command

        public ICommand NavigateToBackupCommand => new Command(async () => await NavigateToBackup());

        public ICommand NavigateToAuthenticationCommand => new Command(async () => await NavigateToAuthentication());

        public ICommand NavigateToLegalPageCommand => new Command(async () => await NavigateToLegalPage());

        public ICommand NavigateToDebugCommand => new Command(async () => await NavigateToDebug());

        #endregion

        #region Bindable Properties

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => this.RaiseAndSetIfChanged(ref _fullName, value);
        }

        private string _avatarUrl;
        public string AvatarUrl
        {
            get => _avatarUrl;
            set => this.RaiseAndSetIfChanged(ref _avatarUrl, value);
        }

        private bool _showDebug;
        public bool ShowDebug
        {
            get => _showDebug;
            set => this.RaiseAndSetIfChanged(ref _showDebug, value);
        }

        private string _appVersion;
        public string AppVersion
        {
            get => _appVersion;
            set => this.RaiseAndSetIfChanged(ref _appVersion, value);
        }

        private string _buildVersion;
        public string BuildVersion
        {
            get => _buildVersion;
            set => this.RaiseAndSetIfChanged(ref _buildVersion, value);
        }

        #endregion
    }
}
