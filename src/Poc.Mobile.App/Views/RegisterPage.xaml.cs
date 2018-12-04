using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Poc.Mobile.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage, IRootView
    {
		public RegisterPage ()
		{
			InitializeComponent ();
		}
	}
}