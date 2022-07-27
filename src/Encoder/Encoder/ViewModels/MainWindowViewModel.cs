using Encoder.Commands;
using Encoder.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encoder.ViewModels
{
    /// <summary>
    /// ViewModel для MainWindow
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Поля и Свойства

        private readonly EncodingConverter _converter;

        // Выбранная кодировка из выпадающего списка
        private string _selectedSourceEncodingName;

        /// <summary>
        /// Выбранная кодировка из выпадающего списка слева
        /// </summary>
        public string SelectedSourceEncodingName
        {
            get => _selectedSourceEncodingName;
            set
            {
                _selectedSourceEncodingName = value;

                OnPropertyChanged(nameof(SelectedSourceEncodingName));
                ConvertCommand.OnCanExecuteChanged();
            }
        }


        /// <summary>
        /// Список всех кодировок для выпадающего списка справа
        /// </summary>
        public IEnumerable<string> EncodingListOutput => AllEncodingNames;

        // Выбранная кодировка из выпадающего списка
        private string _selectedDestinationEncodingName;

        /// <summary>
        /// Выбранная кодировка из выпадающего списка справа
        /// </summary>
        public string SelectedDestinationEncodingName
        {
            get => _selectedDestinationEncodingName;
            set
            {
                _selectedDestinationEncodingName = value;

                OnPropertyChanged(nameof(SelectedDestinationEncodingName));
                ConvertCommand.OnCanExecuteChanged();
            }
        }

        // Вводимый текст до конвертации
        private string _inputText;

        /// <summary>
        /// Вводимый текст до конвертации
        /// </summary>
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;

                OnPropertyChanged(nameof(InputText));
            }
        }

        // Выводимый текст после конвертации
        private string _outputText;

        /// <summary>
        /// Выводимый текст после конвертации
        /// </summary>
        public string OutputText
        {
            get => _outputText;
            set
            {
                _outputText = value;

                OnPropertyChanged(nameof(OutputText));
            }
        }

        // Список сообщений с ошибками
        private List<string> _errorMessages;

        /// <summary>
        /// Список сообщений с ошибками
        /// </summary>
        public List<string> ErrorMessages
        {
            get => _errorMessages;
            set
            {
                _errorMessages = value;

                OnPropertyChanged(nameof(HasErrors));
                OnPropertyChanged(nameof(ErrorMessages));
            }
        }

        /// <summary>
        /// Есть ли ошибки
        /// </summary>
        public bool HasErrors => ErrorMessages.Count > 0;

        /// <summary>
        /// Список всех кодировок
        /// </summary>
        public IEnumerable<string> AllEncodingNames => Encoding.GetEncodings().Select(e => e.Name);

        /// <summary>
        /// Список всех кодировок для выпадающего списка слева
        /// </summary>
        public IEnumerable<string> EncodingListInput => AllEncodingNames;

        #region Комманды

        // Конвертировать
        private CommandBase _convertCommand;

        /// <summary>
        /// Конвертировать
        /// </summary>
        public CommandBase ConvertCommand
        {
            get
            {
                return _convertCommand ?? (_convertCommand =
                    new RelayCommand
                    (
                        execute: (p) => ConvertCommand_Execute(p),
                        canExecute: (p) => ConvertCommand_CanExecute(p)
                    ));
            }
        }


        private CommandBase _tupleCommand;
        public CommandBase TupleCommand
        {
            get
            {
                return _tupleCommand ?? (_tupleCommand =
                    new RelayCommand
                    (
                        execute: (p) => TupleCommand_Execute(p)
                    ));
            }
        }

        // Очистить поля
        private CommandBase _clearFieldsCommand;

        /// <summary>
        /// Очистить поля ввода и вывода
        /// </summary>
        public CommandBase ClearFieldsCommand
        {
            get
            {
                return _clearFieldsCommand ?? (_clearFieldsCommand =
                    new RelayCommand
                    (
                        execute: (p) => { InputText = OutputText = string.Empty; }
                    ));
            }
        }

        #endregion // Комманды

        #endregion // Поля и Свойства

        public MainWindowViewModel()
        {
            _selectedSourceEncodingName = string.Empty;
            _selectedDestinationEncodingName = string.Empty;
            _inputText = string.Empty;
            _outputText = string.Empty;
            _errorMessages = new List<string>();
            _converter = new EncodingConverter();
        }

        #region Методы

        /// <summary>
        /// Поменять текст местами, для метода Execute() комманды TupleCommand
        /// </summary>
        /// <param name="parameter">Параметр комманды</param>
        private void TupleCommand_Execute(object parameter)
        {
            string inputText = this.InputText;
            string outputText = this.OutputText;
            string selectedEncodingSource = this.SelectedSourceEncodingName;
            string selectedEncodingDestination = this.SelectedDestinationEncodingName;

            this.InputText = outputText;
            this.OutputText = inputText;

            this.SelectedSourceEncodingName = selectedEncodingDestination;
            this.SelectedDestinationEncodingName = selectedEncodingSource;
        }

        /// <summary>
        /// Возможно ли произвести конвертациб, для метода CanExecute() команды ConvertCommand
        /// </summary>
        /// <param name="parameter">Параметр комманды</param>
        /// <returns></returns>
        private bool ConvertCommand_CanExecute(object parameter)
        {
            if (!string.IsNullOrEmpty(SelectedSourceEncodingName) && !string.IsNullOrEmpty(SelectedDestinationEncodingName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Конвертировать текст, для метода Execute() комманды ConvertCommand
        /// </summary>
        /// <param name="parameter"></param>
        private void ConvertCommand_Execute(object parameter)
        {
            var result = _converter.Convert
            (
                sourceText: InputText,
                sourceEncodingName: SelectedSourceEncodingName,
                destinationEncodingName: SelectedDestinationEncodingName
            );

            if (result.IsSuccess)
            {
                OutputText = result.Result;

                ErrorMessages = new List<string>();
            }
            else if (result.HasExceptions)
            {
                var errorMessages = new List<string>();

                foreach (var exception in result.Exceptions)
                    errorMessages.Add(exception.Message);

                ErrorMessages = errorMessages;
            }
        }

        #endregion
    }
}
