using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Osma.Mobile.App.Services.Interfaces;
using Osma.Mobile.App.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Osma.Mobile.App.Services
{
    public enum NavigationType
    {
        Normal,
        Modal
    }

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

        public Task NavigateToAsync<TViewModel>(TViewModel viewModel, object parameter = null, NavigationType type = NavigationType.Normal) where TViewModel : IABaseViewModel => InternalNavigateToAsync(typeof(TViewModel), type, viewModel, null);
        
        public Task NavigateToAsync<TViewModel>(object parameter, NavigationType type = NavigationType.Normal) where TViewModel : IABaseViewModel => InternalNavigateToAsync(typeof(TViewModel), type, null, parameter);
        
        public async Task NavigateToAsync(Page page, NavigationType type = NavigationType.Normal)
        {
            var mainPage = CurrentApplication.MainPage;

            if (type == NavigationType.Modal)
                await mainPage.Navigation.PushModalAsync(page);
            else
                await mainPage.Navigation.PushAsync(page);
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage != null)
                await CurrentApplication.MainPage.Navigation.PopAsync();
        }

        public async Task PopModalAsync() => await CurrentApplication.MainPage.Navigation.PopModalAsync();

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
                await PopupNavigation.Instance.PushAsync(page as PopupPage, animate);
            else
                throw new ArgumentException($"The type ${typeof(TViewModel)} its not a PopupPage type");

        }

        public async Task CloseAllPopupsAsync() => await PopupNavigation.Instance.PopAllAsync(true);

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, NavigationType type, object viewModel, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, viewModel, parameter, false);

            if (type == NavigationType.Modal)
                    await CurrentApplication.MainPage.Navigation.PushModalAsync(page);
            else
            {
                if (page is IRootView)
                {
                    object bindingContext = page.BindingContext;
                    page = new NavigationPage(page);
                    NavigationPage.SetHasNavigationBar(page, false);
                    page.BindingContext = bindingContext;
                    CurrentApplication.MainPage = page;
                }
                else if (CurrentApplication.MainPage is NavigationPage navPage)
                    await navPage.Navigation.PushAsync(page);
                //TODO OS-194 else throw exception as the page and Navigation type combination isnt valid
            }

            if (page.BindingContext is IABaseViewModel vm)
                await vm.InitializeAsync(parameter);
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

            //TODO OS-195 throw an exception if this resolution was null i.e the bound type is not of type Page
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
