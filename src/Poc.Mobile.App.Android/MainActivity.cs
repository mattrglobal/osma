using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Autofac;
using FFImageLoading.Forms.Platform;
using Java.Lang;
using Poc.Mobile.App.Converters;
using Xamarin.Forms;

namespace Poc.Mobile.App.Droid
{
    [Activity(Label = "Poc.Mobile.App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);

            // Initializing FFImageLoading
            CachedImageRenderer.Init(false);

            // Initializing User Dialogs
            UserDialogs.Init(this);

            // Initializing Xamarin Essentials
            Xamarin.Essentials.Platform.Init(this, bundle);

#if GORILLA
            LoadApplication(UXDivers.Gorilla.Droid.Player.CreateApplication(
                this,
                new UXDivers.Gorilla.Config("Good Gorilla")
                .RegisterAssemblyFromType<InverseBooleanConverter>()
                .RegisterAssemblyFromType<CachedImageRenderer>()));
#else
            //Loading dependent libindy
            JavaSystem.LoadLibrary("gnustl_shared");
            JavaSystem.LoadLibrary("indy");

            // Initializing QR Code Scanning support
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            //Marshmellow and above require permission requests to be made at runtime
            if ((int)Build.VERSION.SdkInt >= 23)
                CheckAndRequestRequiredPermissions();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new PlatformModule());
            var container = builder.Build();

            LoadApplication(new App(container));
#endif
        }

        readonly string[] _permissionsRequired =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };

        private int _requestCode = -1;
        private List<string> _permissionsToBeGranted = new List<string>();

        private void CheckAndRequestRequiredPermissions()
        {
            for (int i = 0; i < _permissionsRequired.Length; i++)
                if (CheckSelfPermission(_permissionsRequired[i]) != (int)Permission.Granted)
                    _permissionsToBeGranted.Add(_permissionsRequired[i]);

            if (_permissionsToBeGranted.Any())
            {
                _requestCode = 10;
                RequestPermissions(_permissionsRequired.ToArray(), _requestCode);
            }
            else
                System.Diagnostics.Debug.WriteLine("Device already has all the required permissions");
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            Permission[] grantResults)
        {
            if (grantResults.Length == _permissionsToBeGranted.Count)
                System.Diagnostics.Debug.WriteLine("All permissions required that werent granted, have now been granted");
            else
                System.Diagnostics.Debug.WriteLine("Some permissions requested were denied by the user");
           
           Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
           base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

