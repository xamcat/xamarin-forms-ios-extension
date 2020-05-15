using System.Threading.Tasks;

namespace FormsShareExtension
{
    public interface IDialogService
    {
        void SetContext(object context);
        Task ShowDialogAsync(string title, string message);
    }
}
