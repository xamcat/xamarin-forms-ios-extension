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

        private ICommand _doCommand;
        public ICommand DoCommand
        {
            get { return _doCommand; }
            set
            {
                if(_doCommand != value)
                {
                    _doCommand = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DoCommand)));
                }
            }
        }

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
