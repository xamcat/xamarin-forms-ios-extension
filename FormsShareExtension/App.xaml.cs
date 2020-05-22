using System;
using FormsShareExtension.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsShareExtension
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            App.Current.Resources.LoadAppResources();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
