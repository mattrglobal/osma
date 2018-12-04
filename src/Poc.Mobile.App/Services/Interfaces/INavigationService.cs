using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poc.Mobile.App.Services.Interfaces
{
    public interface INavigationService
    {
        Task NavigateToAsync<TViewModel>() where TViewModel : IABaseViewModel;

        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : IABaseViewModel;

        Task NavigateToAsync<TViewModel>(TViewModel viewModel) where TViewModel : IABaseViewModel;

        Task AddTabChildToMainView<TViewModel>(TViewModel viewModel, object parameter, int atIndex = -1) where TViewModel : IABaseViewModel;

        Task NavigateToAsync(Type viewModelType);

        Task NavigateToAsync(Type viewModelType, object parameter);

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
