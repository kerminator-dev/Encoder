using Encoder.Builders;
using EncodingLibrary.Commands;
using EncodingLibrary.Extensions;
using System;
using System.Linq;
using System.Text;

namespace Encoder.Commands
{
    internal class DetectEncodingCommand : CommandBase
    {
        private readonly IDialogService _dialogService;
        private readonly Action<Exception> _onException;
        private readonly Action<string> _onEncodingSelected;

        public DetectEncodingCommand(IDialogService dialogService, Action<Exception> onException, Action<string> onEncodingSelected)
        {
            _dialogService = dialogService;
            _onException = onException;
            _onEncodingSelected = onEncodingSelected;
        }

        public override void Execute(object parameter)
        {
            if (!(parameter is string))
                return;

            var text = parameter as string;

            try
            {
                // Определение кодировки
                var encodings = text.DetectEncodings().Select(e => e.HeaderName).ToList();

                if (encodings == null || encodings.Count == 0)
                {
                    // Если не подобралась кодировка
                    throw new Exception("Не удалось определить подходящую кодировку!");
                }

                string selectedEncoding = string.Empty;
                
                if (encodings.Count == 1)
                {
                    // Если подобралась одна кодировка

                    selectedEncoding = Encoding.GetEncodings().First(e => e.Name == encodings.First()).Name;

                    if (string.IsNullOrEmpty(selectedEncoding))
                        throw new Exception("Не удалось определить подходящую кодировку!");

                    _onEncodingSelected?.Invoke(selectedEncoding);

                    return;
                }
                else
                {
                    // Если подобралось несколько кодировок
                    var command = new OpenWindowSelectEncodingCommand
                    (
                        dialogService: _dialogService,
                        onEncodingSelected: _onEncodingSelected,
                        encodings: encodings
                    );

                    if (command.CanExecute(null))
                        command.Execute(null);
                }
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }
        }
    }
}
