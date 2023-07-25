using Encoder.Collections;
using EncodingLibrary.ViewModels;
using System;
using System.Threading.Tasks;

namespace Encoder.ViewModels
{
    internal class ErrorMessagesViewModel : ViewModelBase
    {
        private const int ADD_DELAY = 200;

        private UniqueObservableCollection<string> _errorMessages;
        public UniqueObservableCollection<string> ErrorMessages
        {
            get => _errorMessages;
            set
            {
                _errorMessages = value;

                OnPropertyChanged(nameof(HasErrors));
                OnPropertyChanged(nameof(ErrorMessages));
            }
        }

        public bool HasErrors => ErrorMessages.Count > 0;

        public ErrorMessagesViewModel()
        {
            _errorMessages = new UniqueObservableCollection<string>();
        }

        public void AddException(Exception exception)
        {
            _errorMessages.Add(exception.Message);
        }

        public async Task AddExceptionsAsync(params Exception[] exceptions)
        {
            this.Clear();

            foreach (var exception in exceptions)
            {
                this.AddException(exception);

                await Task.Delay(ADD_DELAY);
            }
        }

        public void Clear()
        {
            _errorMessages.Clear();
        }
    }
}
