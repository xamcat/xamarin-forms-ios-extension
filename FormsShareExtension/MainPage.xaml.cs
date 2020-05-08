using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FormsShareExtension
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public string TitleText
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
