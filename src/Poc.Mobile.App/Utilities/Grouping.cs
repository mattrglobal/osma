using System.Collections.Generic;
using System.Linq;
using Poc.Mobile.App.Extensions;
using Poc.Mobile.App.ViewModels.Credentials;

namespace Poc.Mobile.App.Utilities
{
    public class Grouping<K, T> : RangeEnabledObservableCollection<T>
    {
        private readonly string key;
        private readonly IGrouping<string, CredentialViewModel> grouped;

        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            InsertRange(items);
        }

        public Grouping(string key, IGrouping<string, CredentialViewModel> grouped)
        {
            this.key = key;
            this.grouped = grouped;
        }
    }
}
