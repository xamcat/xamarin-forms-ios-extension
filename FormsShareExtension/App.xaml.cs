using FormsShareExtension.Resources;
using Xamarin.Forms;

namespace FormsShareExtension
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Application.Current.Resources.LoadAppResources();
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
