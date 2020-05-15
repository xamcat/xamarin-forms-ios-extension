using System.Threading.Tasks;
using FormsShareExtension.iOS.Core.Services;
using UIKit;

namespace FormsShareExtension.iOS.Core.Services
{

    public class DialogService : IDialogService
    {
        private UIViewController _contextViewController;

        public void SetContext(object context)
        {
            _contextViewController = (UIViewController)context; 
        }

        public Task ShowDialogAsync(string title, string message)
        {
            var hostController = _contextViewController ?? UIApplication.SharedApplication.KeyWindow.RootViewController;
            if (hostController == null)
                return Task.CompletedTask;

            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, null));
            return hostController.PresentViewControllerAsync(alert, true);
        }
    }
}
