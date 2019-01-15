using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Autofac;
using DynamicData;
using DynamicData.Binding;
using Poc.Mobile.App.Extensions;
using Poc.Mobile.App.Services;
using Poc.Mobile.App.Services.Interfaces;
using Poc.Mobile.App.Utilities;
using ReactiveUI;
using Streetcred.Sdk.Contracts;
using Streetcred.Sdk.Models.Records;
using Xamarin.Forms;

namespace Poc.Mobile.App.ViewModels.Credentials
{
    public class CredentialsViewModel : ABaseViewModel
    {
        private readonly ICredentialService _credentialService;
        private readonly IAgentContextService _agentContextService;
        private readonly ILifetimeScope _scope;

        public CredentialsViewModel(
            IUserDialogs userDialogs,
            INavigationService navigationService,
            ICredentialService credentialService,
            IAgentContextService agentContextService,
            ILifetimeScope scope
            ) : base(
                "Credentials",
                userDialogs,
                navigationService
           )
        {

            _credentialService = credentialService;
            _agentContextService = agentContextService;
            _scope = scope;

            this.WhenAnyValue(x => x.SearchTerm)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .InvokeCommand(RefreshCommand);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await RefreshCredentials();
            await base.InitializeAsync(navigationData);
        }

        public async Task RefreshCredentials()
        {
            RefreshingCredentials = true;

            var context = await _agentContextService.GetContextAsync();
            var credentialsRecords = await _credentialService.ListAsync(context.Wallet);

#if DEBUG
            credentialsRecords.Add(new CredentialRecord
            {
                ConnectionId = Guid.NewGuid().ToString().ToLowerInvariant(),
                CredentialDefinitionId = Guid.NewGuid().ToString().ToLowerInvariant(),
                CredentialId = Guid.NewGuid().ToString().ToLowerInvariant(),
                CredentialRevocationId = Guid.NewGuid().ToString().ToLowerInvariant(),
                State = CredentialState.Issued,
            });
            credentialsRecords.Add(new CredentialRecord
            {
                ConnectionId = Guid.NewGuid().ToString().ToLowerInvariant(),
                CredentialDefinitionId = Guid.NewGuid().ToString().ToLowerInvariant(),
                CredentialId = Guid.NewGuid().ToString().ToLowerInvariant(),
                CredentialRevocationId = Guid.NewGuid().ToString().ToLowerInvariant(),
                State = CredentialState.Issued,
            });
            credentialsRecords.Add(new CredentialRecord
            {
                ConnectionId = Guid.NewGuid().ToString().ToLowerInvariant(),
                CredentialDefinitionId = Guid.NewGuid().ToString().ToLowerInvariant(),
                CredentialId = Guid.NewGuid().ToString().ToLowerInvariant(),
                CredentialRevocationId = Guid.NewGuid().ToString().ToLowerInvariant(),
                State = CredentialState.Issued,
            });
#endif

            IList<CredentialViewModel> credentialsVms = new List<CredentialViewModel>();
            foreach (var credentialRecord in credentialsRecords)
            {
                CredentialViewModel credential = _scope.Resolve<CredentialViewModel>(new NamedParameter("credential", credentialRecord));
                credentialsVms.Add(credential);
            }

            var filteredCredentialVms = FilterCredentials(SearchTerm, credentialsVms);
            var groupedVms = GroupCredentials(filteredCredentialVms);
            CredentialsGrouped = groupedVms;

            Credentials.Clear();
            Credentials.InsertRange(filteredCredentialVms);

            HasCredentials = Credentials.Any();
            RefreshingCredentials = false;

        }

        public async Task SelectCredential(CredentialViewModel credential) => await NavigationService.NavigateToAsync(credential, null, NavigationType.Modal);

        private IEnumerable<CredentialViewModel> FilterCredentials(string term, IEnumerable<CredentialViewModel> credentials)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return credentials;
            }
            // Basic search
            var filtered = credentials.Where(credentialViewModel => credentialViewModel.CredentialName.Contains(term));
            return filtered;
        }

        private IEnumerable<Grouping<string, CredentialViewModel>> GroupCredentials(IEnumerable<CredentialViewModel> credentialViewModels)
        {
            var grouped = credentialViewModels
            .OrderBy(credentialViewModel => credentialViewModel.CredentialName)
            .GroupBy(credentialViewModel =>
            {
                if(string.IsNullOrWhiteSpace(credentialViewModel.CredentialName))
                {
                    return "*";
                }
                return credentialViewModel.CredentialName[0].ToString().ToUpperInvariant();
            }) // TODO check credentialName
            .Select(group =>
            {
                return new Grouping<string, CredentialViewModel>(group.Key, group.ToList());
            }
            );

            return grouped;

        }



        #region Bindable Command
        public ICommand SelectCredentialCommand => new Command<CredentialViewModel>(async (credentials) =>
        {
            if (credentials != null)
                await SelectCredential(credentials);
        });

        public ICommand RefreshCommand => new Command(async () => await RefreshCredentials());

        #endregion

        #region Bindable Properties
        private RangeEnabledObservableCollection<CredentialViewModel> _credentials = new RangeEnabledObservableCollection<CredentialViewModel>();
        public RangeEnabledObservableCollection<CredentialViewModel> Credentials
        {
            get => _credentials;
            set => this.RaiseAndSetIfChanged(ref _credentials, value);
        }

        private bool _hasCredentials;
        public bool HasCredentials
        {
            get => _hasCredentials;
            set => this.RaiseAndSetIfChanged(ref _hasCredentials, value);
        }

        private bool _refreshingCredentials;
        public bool RefreshingCredentials
        {
            get => _refreshingCredentials;
            set => this.RaiseAndSetIfChanged(ref _refreshingCredentials, value);
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set => this.RaiseAndSetIfChanged(ref _searchTerm, value);
        }

        private IEnumerable<Grouping<string, CredentialViewModel>> _credentialsGrouped;
        public IEnumerable<Grouping<string, CredentialViewModel>> CredentialsGrouped
        {
            get => _credentialsGrouped;
            set => this.RaiseAndSetIfChanged(ref _credentialsGrouped, value);
        }

        #endregion
    }
}
