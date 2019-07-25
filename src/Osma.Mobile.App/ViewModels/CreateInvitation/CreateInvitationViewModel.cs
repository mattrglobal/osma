using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using AgentFramework.Core.Contracts;
using Osma.Mobile.App.Services.Interfaces;
using Osma.Mobile.App.ViewModels.Account;
using ReactiveUI;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using AgentFramework.Core.Extensions;

namespace Osma.Mobile.App.ViewModels.CreateInvitation
{
    public class CreateInvitationViewModel : ABaseViewModel
    {
        private readonly ICustomAgentContextProvider _agentContextProvider;
        private readonly IConnectionService _connectionService;

        public CreateInvitationViewModel(
            IUserDialogs userDialogs,
            INavigationService navigationService,
            ICustomAgentContextProvider agentContextProvider,
            IConnectionService defaultConnectionService
            ) : base(
                "CreateInvitation",
                userDialogs,
                navigationService
           )
        {
            _agentContextProvider = agentContextProvider;
            _connectionService = defaultConnectionService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await base.InitializeAsync(navigationData);
        }

        private async Task CreateInvitation()
        {
            try
            {
                var context = await _agentContextProvider.GetContextAsync();
                var (invitation, _) = await _connectionService.CreateInvitationAsync(context);

                string barcodeValue = invitation.ServiceEndpoint + "?c_i=" + (invitation.ToJson().ToBase64());
                QrCodeValue = barcodeValue;
            }
            catch (Exception ex)
            {
                DialogService.Alert(ex.Message);
            }
        }

        private ZXingBarcodeImageView QRCodeGenerator(String barcodeValue)
        {
            var barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingBarcodeImageView",
            };

            barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            barcode.BarcodeOptions.Width = 300;
            barcode.BarcodeOptions.Height = 300;
            barcode.BarcodeOptions.Margin = 10;
            barcode.BarcodeValue = barcodeValue;

            return barcode;

        }

        #region Bindable Command

        public ICommand CreateInvitationCommand => new Command(async () => await CreateInvitation());

        #endregion

        #region Bindable Properties

        private string _qrCodeValue;

        public string QrCodeValue
        {
            get => _qrCodeValue;
            set => this.RaiseAndSetIfChanged(ref _qrCodeValue, value);
        }

        #endregion
    }
}
