using Encoder.Collections;
using Encoder.Commands;
using Encoder.Builders;
using Encoder.ViewModels;
using EncodingLibrary.Commands;
using EncodingLibrary.Converters;
using EncodingLibrary.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodingLibrary.ViewModels
{
    /// <summary>
    /// ViewModel для MainWindow
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private ErrorMessagesViewModel _errorMessagesViewModel;

        // Выбранная кодировка из выпадающего списка
        private string _selectedInputEncodingName;

        /// <summary>
        /// Выбранная кодировка из выпадающего списка слева
        /// </summary>
        public string SelectedInputEncodingName
        {
            get => _selectedInputEncodingName;
            set
            {
                _selectedInputEncodingName = value;

                OnPropertyChanged(nameof(SelectedInputEncodingName));
            }
        }


        // Выбранная кодировка из выпадающего списка
        private string _selectedOutputEncodingName;

        /// <summary>
        /// Выбранная кодировка из выпадающего списка справа
        /// </summary>
        public string SelectedOutputEncodingName
        {
            get => _selectedOutputEncodingName;
            set
            {
                _selectedOutputEncodingName = value;

                OnPropertyChanged(nameof(SelectedOutputEncodingName));
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

        /// <summary>
        /// Список сообщений с ошибками
        /// </summary>
        public UniqueObservableCollection<string> ErrorMessages
        {
            get => _errorMessagesViewModel.ErrorMessages;
            set => _errorMessagesViewModel.ErrorMessages = value;
        }

        /// <summary>
        /// Есть ли ошибки
        /// </summary>
        public bool HasErrors => _errorMessagesViewModel.HasErrors;

        private string[] _allEncodingNames;
        public string[] AllEncodingNames
        {
            get
            {
                if (_allEncodingNames == null)
                {
                    _allEncodingNames = Encoding.GetEncodings()
                                                .OrderByDescending(e => e.Name)
                                                .Select(e => e.Name)
                                                .ToArray();
                }

                return _allEncodingNames;
            }
        }


        // Конвертировать
        private CommandBase _convertCommand;

        /// <summary>
        /// Конвертировать
        /// </summary>
        public CommandBase ConvertCommand
        {
            get
            {
                if (_convertCommand == null)
                {
                    _convertCommand = new RelayCommand
                    (
                        execute: async (p) => await ConvertCommand_Execute(p)
                    );
                }

                return _convertCommand;
            }
        }

        private CommandBase _tupleEncodingsCommand;

        public CommandBase TupleEncodingsCommand
        {
            get
            {
                if (_tupleEncodingsCommand == null)
                {
                    _tupleEncodingsCommand = new RelayCommand
                    (
                        execute: (p) => TupleEncodingsCommand_Execute(p)
                    );  
                }

                return _tupleEncodingsCommand;
            }
        }


        private CommandBase _tupleCommand;
        public CommandBase TupleCommand
        {
            get
            {
                if (_tupleCommand == null)
                {
                    _tupleCommand = new RelayCommand
                    (
                        execute: (p) => TupleTextCommand_Execute(p)
                    );
                }

                return _tupleCommand;
            }
        }

        private CommandBase _clearFieldsCommand;

        /// <summary>
        /// Очистить поля ввода и вывода
        /// </summary>
        public CommandBase ClearFieldsCommand
        {
            get
            {
                if (_clearFieldsCommand == null)
                {
                    _clearFieldsCommand = new RelayCommand
                    (
                        execute: (p) => { InputText = OutputText = string.Empty; }
                    );
                }

                return _clearFieldsCommand;
            }
        }

        private CommandBase _openFileCommand;

        /// <summary>
        /// Открыть файл
        /// </summary>
        public CommandBase OpenFileCommand
        {
            get
            {    
                if (_openFileCommand == null)
                {
                    _openFileCommand = new OpenFileContentCommand
                     (
                         onFileOpened: (fileInfo) =>
                         {
                             InputText = fileInfo.Content;
                             SelectedInputEncodingName = fileInfo.Encoding;
                         },
                         onException: _errorMessagesViewModel.AddException
                     );
                }

                return _openFileCommand;
            }
        }

        private CommandBase _detectInputEncodingCommand;

        /// <summary>
        /// Определить исходную кодировку текста
        /// </summary>
        public CommandBase DetectInputEncodingCommand
        {
            get
            {
                return _detectInputEncodingCommand;
            }
        }

        private CommandBase _openWindowAboutProgramCommand;

        public CommandBase OpenWindowAboutProgramCommand
        {
            get
            {
                if (_openWindowAboutProgramCommand == null)
                {
                    _openWindowAboutProgramCommand = new OpenWindowAboutProgramCommand();
                }

                return _openWindowAboutProgramCommand;
            }
        }


        public MainWindowViewModel(IDialogService dialogService)
        {
            _selectedInputEncodingName = string.Empty;
            _selectedOutputEncodingName = string.Empty;
            _inputText = string.Empty;
            _outputText = string.Empty;
            _errorMessagesViewModel = new ErrorMessagesViewModel();

            _detectInputEncodingCommand = new DetectEncodingCommand
            (
                dialogService,
                onException: (ex) => _errorMessagesViewModel.AddException(ex),
                onEncodingSelected: (encoding) => SelectedInputEncodingName = encoding
            );

            ErrorMessages.CollectionChanged += ErrorMessages_CollectionChanged;
        }

        private void ErrorMessages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(ErrorMessages));
        }

        #region Методы для команд

        /// <summary>
        /// Поменять текст местами, для метода Execute() команды TupleCommand
        /// </summary>
        /// <param name="parameter">Параметр комманды</param>
        private void TupleTextCommand_Execute(object parameter)
        {
            var temp = this.InputText;
            this.InputText = this.OutputText;
            this.OutputText = temp;
        }

        private void TupleEncodingsCommand_Execute(object parameter)
        {
            var temp = String.IsNullOrEmpty(this.SelectedInputEncodingName) ? null : this.SelectedInputEncodingName;
            this.SelectedInputEncodingName = String.IsNullOrEmpty(this.SelectedOutputEncodingName) ? null : this.SelectedOutputEncodingName;
            this.SelectedOutputEncodingName = temp;
        }

        /// <summary>
        /// Конвертировать текст, для метода Execute() комманды ConvertCommand
        /// </summary>
        /// <param name="parameter"></param>
        private async Task ConvertCommand_Execute(object parameter)
        {
            if (!(parameter is string))
                return;

            var text = parameter as string;

            try
            {
                OutputText = text.ChangeEncoding
                (
                    sourceEncodingName: SelectedInputEncodingName,
                    destinationEncodingName: SelectedOutputEncodingName,
                    encodingConverter: new DefaultEncodingConverter()
                );

                ErrorMessages?.Clear();
            }
            catch (AggregateException ex)
            {
                await _errorMessagesViewModel.AddExceptionsAsync(ex.InnerExceptions.ToArray());
            }
            catch (Exception ex)
            {
                _errorMessagesViewModel.AddException(ex);
            }
        }

        #endregion
    }
}
