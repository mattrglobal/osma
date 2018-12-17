using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Poc.Mobile.App.Services.Interfaces
{
    public interface INavigationService
    {
        Task PopModalAsync();

        Task NavigateToAsync(Page page, NavigationType type = NavigationType.Normal);

        Task NavigateToAsync<TViewModel>(object parameter = null, NavigationType type = NavigationType.Normal) where TViewModel : IABaseViewModel;
        
        Task NavigateToAsync<TViewModel>(TViewModel viewModel, object parameter = null, NavigationType type = NavigationType.Normal) where TViewModel : IABaseViewModel;

        Task AddTabChildToMainView<TViewModel>(TViewModel viewModel, object parameter, int atIndex = -1) where TViewModel : IABaseViewModel;
        
        Task NavigateBackAsync();

        Task RemoveLastFromBackStackAsync();

        Task NavigateToPopupAsync<TViewModel>(bool animate, TViewModel viewModel) where TViewModel : IABaseViewModel;

        Task NavigateToPopupAsync<TViewModel>(object parameter, bool animate, TViewModel viewModel) where TViewModel : IABaseViewModel;

        Task CloseAllPopupsAsync();

        IList<IABaseViewModel> GetMainViewTabChildren();

        bool RemoveTabChildFromMainView(IABaseViewModel childViewModel);

        void SetCurrentTabOnMainView<TViewModel>();

        Type GetCurrentPageViewModel();

        bool SetCurrentPageTitle(string title);

        void AddPageViewModelBinding<TVm, TP>();

        void AddPopupViewModelBinding<TVm, TV>();
    }
}
