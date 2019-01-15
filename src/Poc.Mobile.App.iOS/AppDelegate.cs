using Autofac;
using FFImageLoading.Forms.Platform;
using Foundation;
using Poc.Mobile.App.Converters;
using Poc.Mobile.App.Services;
using UIKit;

namespace Poc.Mobile.App.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();

            // Initializing FFImageLoading
            CachedImageRenderer.Init();

#if GORILLA

            LoadApplication(UXDivers.Gorilla.iOS.Player.CreateApplication(
                new UXDivers.Gorilla.Config("Good Gorilla")
                    .RegisterAssemblyFromType<InverseBooleanConverter>()
                    .RegisterAssemblyFromType<FFImageLoading.Transformations.CircleTransformation>()
                    .RegisterAssemblyFromType<FFImageLoading.Forms.CachedImage>()
                ));
#else
            // Initializing QR Code Scanning support
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new PlatformModule());
            builder.RegisterModule(new ServicesModule());
            var container = builder.Build();
            LoadApplication(new App(container));
#endif
            return base.FinishedLaunching(app, options);
        }
    }
}
