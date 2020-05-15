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

        public bool IsDoneAvailable => DoneCommand != null;

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

        private ICommand _doneCommand;
        public ICommand DoneCommand
        {
            get { return _doneCommand; }
            set
            {
                if (_doneCommand != value)
                {
                    _doneCommand = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DoneCommand)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDoneAvailable)));
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
            DependencyService.Get<IDialogService>().ShowDialogAsync("Success", Message);
        }
    }
}
