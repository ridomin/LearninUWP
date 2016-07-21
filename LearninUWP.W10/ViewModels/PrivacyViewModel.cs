using System;
using AppStudio.Uwp;

namespace LearninUWP.ViewModels
{
    public class PrivacyViewModel : ObservableBase
    {
        public Uri Url
        {
            get
            {
                return new Uri(UrlText, UriKind.RelativeOrAbsolute);
            }
        }
        public string UrlText
        {
            get
            {
                return "https://ridophotos.azurewebsites.net/privacy";
            }
        }
    }
}

