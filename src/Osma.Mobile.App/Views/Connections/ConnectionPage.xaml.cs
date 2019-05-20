using System;
using Xamarin.Forms;

namespace Osma.Mobile.App.Views.Connections
{
    public partial class ConnectionPage : ContentPage
    {

        public ConnectionPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        private void ToggleModalTapped(object sender, EventArgs e)
        {
            moreModal.IsVisible = !moreModal.IsVisible;
        }
    }
}
