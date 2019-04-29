using Xamarin.Forms;

namespace Osma.Mobile.App.Utilities
{
    public struct Palette
    {
        public Color BasePageColor;

        public void Init()
        {
            var resources = Application.Current.Resources;
            BasePageColor = (Color)resources["BasePageColor"];
        }
    }
}
