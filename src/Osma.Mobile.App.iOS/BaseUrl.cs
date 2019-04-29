using System;
using Foundation;
using Osma.Mobile.App.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(BaseUrl))]
namespace Osma.Mobile.App.iOS
{
    public class BaseUrl: IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}
