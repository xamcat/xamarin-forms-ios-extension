using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace FormsShareExtension
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

        public ICommand DoCommand { get; }

        public MainPageViewModel()
        {
            DoCommand = new Command(OnDoCommandExecuted);
        }

        private void OnDoCommandExecuted(object state)
        {
            Message = $"Job {Environment.TickCount} has been completed!";
        }
    }
}
