using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace Osma.Mobile.App.Views.Components
{
    public partial class HtmlFromResource : ContentView
    {
        public HtmlFromResource()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty FileNameProperty =
            BindableProperty.Create("FileName", typeof(string), typeof(DetailedCell), "", propertyChanged: FileNamePropertyChanged);


        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        static void FileNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HtmlFromResource view = (HtmlFromResource)bindable;
            Device.BeginInvokeOnMainThread(() =>
            {
                var source = new HtmlWebViewSource();
                string url = DependencyService.Get<IBaseUrl>().Get();
                string TempUrl = Path.Combine(url, "Resources", "legal");
                source.BaseUrl = url;
                string html;
                try
                {
                    using (var sr = new StreamReader(TempUrl))
                    {
                        html = sr.ReadToEnd();
                        source.Html = html;
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
                view.webview.Source = source;
            });
        }
    }
}
