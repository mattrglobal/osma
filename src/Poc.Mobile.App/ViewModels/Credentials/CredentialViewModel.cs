using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Poc.Mobile.App.Services.Interfaces;
using ReactiveUI;
using Xamarin.Forms;
using AgentFramework.Core.Models.Records;

namespace Poc.Mobile.App.ViewModels.Credentials
{
    public class CredentialViewModel : ABaseViewModel
    {
        private readonly CredentialRecord _credential;

        public CredentialViewModel(
            IUserDialogs userDialogs,
            INavigationService navigationService,
            CredentialRecord credential
        ) : base(
            nameof(CredentialViewModel),
            userDialogs,
            navigationService
        )
        {
            _credential = credential;
            _isNew = IsCredentialNew(_credential);

            #if DEBUG
            _credentialName = "Credential Name";
            _credentialImageUrl = "http://placekitten.com/g/200/200";
            _credentialSubtitle = "10/22/2017";
            _credentialType = "Bank Statement";
            _qRImageUrl = "http://placekitten.com/g/100/100";

            var attributes = new List<CredentialAttribute>( new CredentialAttribute[] {
                new CredentialAttribute
                {
                    Type="Text",
                    Name="First Name",
                    Value="Jamie"
                },
                new CredentialAttribute
                {
                    Type="Text",
                    Name="Last Name",
                    Value="Doe"
                },
                new CredentialAttribute
                {
                    Type = "Text",
                    Name = "Country of Residence",
                    Value = "New Zealand"
                },
                new CredentialAttribute
                {
                    Type="File",
                    Name="Statement",
                    Value="Statement.pdf",
                    FileExt="PDF",
                    Date="05 Aug 2018"
                }
            });
            _attributes = attributes
                .OrderByDescending(o=>o.Type).OrderBy(o=>o.Date);
            #endif
        }


        private bool IsCredentialNew(CredentialRecord credential)
        {
            // TODO OS-200, Currently a Placeholder for a mix of new and not new cells
            Random random = new Random();
            return random.Next(0, 2) == 1;
        }

#region Bindable Command
        public ICommand NavigateBackCommand => new Command(async () =>
        {
            await NavigationService.PopModalAsync();
        });
#endregion

#region Bindable Properties
        private string _credentialName;
        public string CredentialName
        {
            get => _credentialName;
            set => this.RaiseAndSetIfChanged(ref _credentialName, value);
        }

        private string _credentialType;
        public string CredentialType
        {
            get => _credentialType;
            set => this.RaiseAndSetIfChanged(ref _credentialType, value);
        }

        private string _credentialImageUrl;
        public string CredentialImageUrl
        {
            get => _credentialImageUrl;
            set => this.RaiseAndSetIfChanged(ref _credentialImageUrl, value);
        }

        private string _credentialSubtitle;
        public string CredentialSubtitle
        {
            get => _credentialSubtitle;
            set => this.RaiseAndSetIfChanged(ref _credentialSubtitle, value);
        }

        private bool _isNew;
        public bool IsNew
        {
            get => _isNew;
            set => this.RaiseAndSetIfChanged(ref _isNew, value);
        }

        private string _qRImageUrl;
        public string QRImageUrl
        {
            get => _qRImageUrl;
            set => this.RaiseAndSetIfChanged(ref _qRImageUrl, value);
        }

        private IEnumerable<CredentialAttribute> _attributes;
        public IEnumerable<CredentialAttribute> Attributes
        {
            get => _attributes;
            set => this.RaiseAndSetIfChanged(ref _attributes, value);
        }

#endregion
    }
}
