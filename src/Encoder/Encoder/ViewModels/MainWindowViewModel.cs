using Encoder.Services;
using Encoder.Utils.Collections;
using Encoder.ViewModels;
using Encoder.Views;
using EncodingLibrary.Commands;
using EncodingLibrary.Extensions;
using Microsoft.Win32;
using System;
using System.IO;
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
        #region Поля и Свойства

        private IDialogService _dialogService;

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
                ConvertCommand.OnCanExecuteChanged();
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
        private UniqueObservableCollection<string> _errorMessages;

        /// <summary>
        /// Список сообщений с ошибками
        /// </summary>
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

        /// <summary>
        /// Есть ли ошибки
        /// </summary>
        public bool HasErrors => ErrorMessages.Count > 0;

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
                    _openFileCommand = new RelayCommand
                     (
                         execute: OpenFileCommand_Execute
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
                if (_detectInputEncodingCommand == null)
                {
                    _detectInputEncodingCommand = new RelayCommand
                    (
                        execute: DetectInputEncodingCommand_Execute
                    );
                }

                return _detectInputEncodingCommand;
            }
        }

        #endregion // Команды

        #endregion // Поля и Свойства

        public MainWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _selectedInputEncodingName = string.Empty;
            _selectedOutputEncodingName = string.Empty;
            _inputText = string.Empty;
            _outputText = string.Empty;
            _errorMessages = new UniqueObservableCollection<string>();

            ErrorMessages.CollectionChanged += ErrorMessages_CollectionChanged;
        }

        private void ErrorMessages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(ErrorMessages));
        }

        #region Методы для команд

        private void DetectInputEncodingCommand_Execute(object parameter)
        {
            try
            {
                if (String.IsNullOrEmpty(InputText))
                {
                    throw new ArgumentNullException("Исходный текст должен иметь содердимое!");
                }

                var encodings = InputText.DetectEncodings().Select(e => e.HeaderName).ToList();

                if (encodings == null || encodings.Count == 0)
                {
                    throw new Exception("Для текста не удалось определить подходящие кодировки!");
                }

                if (encodings.Count == 1)
                {
                    SelectedInputEncodingName = AllEncodingNames.FirstOrDefault(e => e == encodings.First());

                    return;
                }

                _ = _dialogService.ShowDialog<SelectEncodingWindow, SelectEncodingWindowViewModel, string>
                (
                    onCloseCallback: (dialogResult, selectedEncoding) =>
                    {
                        if (dialogResult.HasValue && dialogResult.Value)
                        {
                            SelectedInputEncodingName = selectedEncoding;
                        }
                    },
                    ViewModelparameters: encodings
                );

            }
            catch (Exception ex)
            {
                this.ErrorMessages?.Add(ex.Message);
            }
        }

        private async void OpenFileCommand_Execute(object parameter)
        {
            try
            {
                OpenFileDialog openfileDialog = new OpenFileDialog();
                openfileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|xml Файлы (*.xml)|*.xml|Все файлы (*.*)|*.*";

                if (openfileDialog.ShowDialog() == true)
                {
                    using (StreamReader reader = new StreamReader(openfileDialog.FileName))
                    {
                        InputText = await reader.ReadToEndAsync();
                        SelectedInputEncodingName = reader.CurrentEncoding.HeaderName;
                    }
                }
            }
            catch (Exception ex)
            {
                _errorMessages?.Add(ex.Message);
            }
        }

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
            try
            {
                OutputText = InputText.ChangeEncoding(
                    sourceEncodingName: SelectedInputEncodingName,
                    destinationEncodingName: SelectedOutputEncodingName
                );

                ErrorMessages?.Clear();
            }

            catch (AggregateException ex)
            {
                foreach (var exception in ex.InnerExceptions)
                {
                    ErrorMessages?.Add(ex.Message);

                    await Task.Delay(200);
                }
            }
        }

        #endregion
    }
}
