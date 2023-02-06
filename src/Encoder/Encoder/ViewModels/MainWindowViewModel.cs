using Encoder.Commands;
using Encoder.Converters;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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


        /// <summary>
        /// Список всех кодировок для выпадающего списка справа
        /// </summary>
        public IEnumerable<string> EncodingListOutput => AllEncodingNames;

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
        private ObservableCollection<string> _errorMessages;

        /// <summary>
        /// Список сообщений с ошибками
        /// </summary>
        public ObservableCollection<string> ErrorMessages
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

        public IEnumerable<string> AllEncodingNames { get; }

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
                        execute: async (p) => await ConvertCommand_Execute(p)
                    ));
            }
        }

        private CommandBase _tupleEncodingsCommand;

        public CommandBase TupleEncodingsCommand
        {
            get
            {
                return _tupleEncodingsCommand ?? (_tupleEncodingsCommand =
                   new RelayCommand
                   (
                       execute: (p) => TupleEncodingsCommand_Execute(p)
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
                        execute: (p) => TupleTextCommand_Execute(p)
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

        // Открыть файл
        private CommandBase _openFileCommand;

        /// <summary>
        /// Открыть файл
        /// </summary>
        public CommandBase OpenFileCommand
        {
            get
            {
                return _openFileCommand ?? (_openFileCommand =
                    new RelayCommand
                    (
                        execute: OpenFileCommand_Execute
                    ));
            }
        }

        #endregion // Комманды

        #endregion // Поля и Свойства

        public MainWindowViewModel()
        {
            _selectedInputEncodingName = string.Empty;
            _selectedOutputEncodingName = string.Empty;
            _inputText = string.Empty;
            _outputText = string.Empty;
            _errorMessages = new ObservableCollection<string>();
            _converter = new EncodingConverter();
            AllEncodingNames = Encoding.GetEncodings().Select(e => e.Name).ToList();

            ErrorMessages.CollectionChanged += ErrorMessages_CollectionChanged;
        }

        private void ErrorMessages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(ErrorMessages));
        }

        #region Методы для команд



        /// <summary>
        /// Открыть содержимое файла, для метода Execute() команды OpenFileCommand
        /// </summary>
        /// <param name="parameter"></param>
        private void OpenFileCommand_Execute(object parameter)
        {
            try
            {
                OpenFileDialog openfileDialog = new OpenFileDialog();
                openfileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|xml Файлы (*.xml)|*.xml|Все файлы (*.*)|*.*";

                if (openfileDialog.ShowDialog() == true)
                {
                    InputText = File.ReadAllText(openfileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ErrorMessages.Add(ex.Message);
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
            var result = _converter.Convert
            (
                sourceText: InputText,
                sourceEncodingName: SelectedInputEncodingName,
                destinationEncodingName: SelectedOutputEncodingName
            );


            ErrorMessages?.Clear();

            if (result.IsSuccess)
            {
                OutputText = result.Result;
            }
            else if (result.HasExceptions)
            {
                foreach (var exception in result.Exceptions)
                {
                    ErrorMessages?.Add(exception.Message);

                    await Task.Delay(200);
                }
            }
        }

        #endregion
    }
}
