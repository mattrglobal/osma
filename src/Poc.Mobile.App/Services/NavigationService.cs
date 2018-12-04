using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Poc.Mobile.App.Services.Interfaces;
using Poc.Mobile.App.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Poc.Mobile.App.Services
{
    public class NavigationService : INavigationService
    {
        private readonly ILifetimeScope _scope;
        protected readonly IList<Tuple<Type, Type, bool>> Mappings;

        protected Application CurrentApplication => Application.Current;
        
        public NavigationService(ILifetimeScope scope)
        {
            _scope = scope;
            Mappings = new List<Tuple<Type, Type, bool>>();
        }
        
        public void SetCurrentTabOnMainView<TViewModel>()
        {
            var mainPage = CurrentApplication.MainPage as TabbedPage;

            if (mainPage == null)
            {
                var navPageWrapper = CurrentApplication.MainPage as NavigationPage;

                if (navPageWrapper != null)
                    mainPage = navPageWrapper.CurrentPage as TabbedPage;
            }

            if (mainPage == null)
                throw new Exception("The current page is not a tabbed page therefore cannot add a child");

            Type viewModelType = typeof(TViewModel);

            Page pageToSelect = mainPage.Children.FirstOrDefault(_ => _.BindingContext.GetType() == viewModelType);

            mainPage.CurrentPage = pageToSelect ?? throw new Exception($"No tabbed page attached to the main view which is currently bound to the viewmodel {viewModelType}");
        }

        public async Task AddTabChildToMainView<TViewModel>(TViewModel viewModel, object parameter, int atIndex = -1) where TViewModel : IABaseViewModel
        {
            var mainPage = CurrentApplication.MainPage as TabbedPage;

            if (mainPage == null)
            {
                var navPageWrapper = CurrentApplication.MainPage as NavigationPage;

                if (navPageWrapper != null)
                    mainPage = navPageWrapper.CurrentPage as TabbedPage;
            }

            if (mainPage == null)
                throw new Exception("The current page is not a tabbed page therefore cannot add a child");

            Type viewModelType = typeof(TViewModel);

            Page page = CreateAndBindPage(viewModelType, viewModel, parameter, false);

            if (atIndex >= 0)
                mainPage.Children.Insert(atIndex, page);
            else
                mainPage.Children.Add(page);

            await (page.BindingContext as IABaseViewModel).InitializeAsync(parameter);
        }

        public IList<IABaseViewModel> GetMainViewTabChildren()
        {
            var mainPage = CurrentApplication.MainPage as TabbedPage;

            if (mainPage == null)
            {
                if (CurrentApplication.MainPage is NavigationPage navPageWrapper)
                    mainPage = navPageWrapper.CurrentPage as TabbedPage;
            }

            if (mainPage == null)
                throw new Exception("The current page is either null or not a tabbed page cannot inspect children");

            return mainPage.Children.Select(_ => (IABaseViewModel)_.BindingContext).ToList();
        }

        public bool RemoveTabChildFromMainView(IABaseViewModel childViewModel)
        {
            var mainPage = CurrentApplication.MainPage as TabbedPage;

            if (mainPage == null)
            {
                if (CurrentApplication.MainPage is NavigationPage navPageWrapper)
                    mainPage = navPageWrapper.CurrentPage as TabbedPage;
            }

            if (mainPage == null)
                throw new Exception("The current page is either null or not a tabbed page cannot inspect children");

            var childPage = mainPage.Children.FirstOrDefault(_ => (IABaseViewModel)_.BindingContext == childViewModel);

            if (childPage != null)
                mainPage.Children.Remove(childPage);

            return true;
        }

        public Task NavigateToAsync<TViewModel>(TViewModel viewModel) where TViewModel : IABaseViewModel => InternalNavigateToAsync(typeof(TViewModel), viewModel, null);

        public Task NavigateToAsync<TViewModel>() where TViewModel : IABaseViewModel => InternalNavigateToAsync(typeof(TViewModel), null, null);

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : IABaseViewModel => InternalNavigateToAsync(typeof(TViewModel), null, parameter);

        public Task NavigateToAsync(Type viewModelType) => InternalNavigateToAsync(viewModelType, null, null);

        public Task NavigateToAsync(Type viewModelType, object parameter) => InternalNavigateToAsync(viewModelType, null, parameter);

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage != null)
                await CurrentApplication.MainPage.Navigation.PopAsync();
        }

        public Type GetCurrentPageViewModel()
        {
            if (CurrentApplication.MainPage != null)
            {
                Page currentPage = CurrentApplication.MainPage.Navigation.NavigationStack.Last();
                if (currentPage?.BindingContext != null)
                    return currentPage.BindingContext.GetType();
            }
            return null;
        }

        public bool SetCurrentPageTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Page currentPage = CurrentApplication.MainPage.Navigation.NavigationStack.Last();
                if (currentPage != null)
                {
                    currentPage.Title = title;
                    return true;
                }
            }
            return false;
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            var mainPage = CurrentApplication.MainPage as Page;

            mainPage.Navigation.RemovePage(
                mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);

            return Task.FromResult(true);
        }

        public Task NavigateToPopupAsync<TViewModel>(bool animate, TViewModel viewModel) where TViewModel : IABaseViewModel => NavigateToPopupAsync(null, animate, viewModel);

        public async Task NavigateToPopupAsync<TViewModel>(object parameter, bool animate, TViewModel vieModel) where TViewModel : IABaseViewModel
        {
            var page = CreateAndBindPage(typeof(TViewModel), vieModel, parameter, true);

            await (page.BindingContext as IABaseViewModel)?.InitializeAsync(parameter);

            if (page is PopupPage)
                await PopupNavigation.PushAsync(page as PopupPage, animate);
            else
                throw new ArgumentException($"The type ${typeof(TViewModel)} its not a PopupPage type");

        }

        public async Task CloseAllPopupsAsync() => await PopupNavigation.PopAllAsync(true);

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object viewModel, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, viewModel, parameter, false);

            if (page is IRootView)
            {
                //TODO prob need to check the navigation stack at this point to ensure there are no pages on top??
                if (page is IMainView)
                {
                    object bindingContext = page.BindingContext;
                    page = new NavigationPage(page);
                    page.BindingContext = bindingContext;
                }

                CurrentApplication.MainPage = page;
            }
            else if (CurrentApplication.MainPage is IMainView || CurrentApplication.MainPage is NavigationPage) // Implemented as an interface so we can have different main views on different platforms
            {
                var mainPage = CurrentApplication.MainPage as Page;

                if (mainPage.GetType() != page.GetType())
                    await mainPage.Navigation.PushAsync(page);
            }

            await (page.BindingContext as IABaseViewModel).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType, bool isPopup)
        {
            var pageType = !isPopup ? Mappings.FirstOrDefault(_ => (_.Item1 == viewModelType) && !_.Item3).Item2 : 
                Mappings.FirstOrDefault(_ => (_.Item1 == viewModelType) && _.Item3).Item2;

            if (pageType == null)
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");

            return pageType;
        }

        protected Page CreateAndBindPage(Type viewModelType, object viewModelObj, object parameter, bool isPopup)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType, isPopup);

            if (pageType == null)
                throw new Exception($"Mapping type for {viewModelType} is not a page");

            Page page = _scope.Resolve(pageType) as Page;
            
            IABaseViewModel viewModel;
            if (viewModelObj != null)
                viewModel = viewModelObj as IABaseViewModel;
            else
                viewModel = _scope.Resolve(viewModelType) as IABaseViewModel;
            page.BindingContext = viewModel;
            
            return page;
        }

        public void AddPageViewModelBinding<TVm, TP>() => Mappings.Add(new Tuple<Type, Type, bool>(typeof(TVm), typeof(TP), false));

        public void AddPopupViewModelBinding<TVm, TV>() => Mappings.Add(new Tuple<Type, Type, bool>(typeof(TVm), typeof(TV), true));
    }
}
