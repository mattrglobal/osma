using System;
using Osma.Mobile.App.ViewModels.Connections;
using Xamarin.Forms;


namespace Osma.Mobile.App.Views.Connections
{
    public class ConnectionTransactionTemplateSelector : DataTemplateSelector
    {

        public DataTemplate ActionableTemplate { get; set; }
        public DataTemplate StatusTemplate { get; set; }
        public DataTemplate ErrorTemplate { get; set; }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is null)
            {
                // TODO Add Logging 
                return ErrorTemplate;
            }

            TransactionItemType transactionItemType;
            TransactionItem transactionItem = item as TransactionItem;

            if (transactionItem is null)
            {
                // TODO Add Logging
                return ErrorTemplate;
            }

            try
            {
                transactionItemType = (TransactionItemType)Enum.Parse(typeof(TransactionItemType), transactionItem.Type);
            }
            catch (ArgumentException)
            {
                // TODO Add Logging TransactionItem
                throw new ArgumentException("Transaction Item Type is Invalid");
            }

            switch (transactionItemType)
            {
                case TransactionItemType.Actionable:
                    return ActionableTemplate;
                case TransactionItemType.Status:
                    return StatusTemplate;
                default:
                    return ErrorTemplate;
            }
        }
    }
}
