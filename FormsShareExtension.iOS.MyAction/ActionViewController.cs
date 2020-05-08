using System;
using Foundation;
using UIKit;
using FormsShareExtension;
using Xamarin.Forms;

namespace MyAction
{
    public partial class ActionViewController : UIViewController
    {
        protected ActionViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Create XF page and display it to end user
            global::Xamarin.Forms.Forms.Init();
            var xfPage = new MainPage();
            xfPage.TitleText = "Welcome to XF Page created from an iOS extension";
            var newController = xfPage.CreateViewController();
            newController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            this.PresentModalViewController(newController, false);
        }

        partial void DoneClicked(NSObject sender)
        {
            // Return any edited content to the host app.
            // This template doesn't do anything, so we just echo the passed-in items.
            ExtensionContext.CompleteRequest(ExtensionContext.InputItems, null);
        }
    }
}
