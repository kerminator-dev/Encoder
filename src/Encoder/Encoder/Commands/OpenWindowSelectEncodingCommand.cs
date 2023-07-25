using Encoder.Builders;
using Encoder.ViewModels;
using Encoder.Views;
using EncodingLibrary.Commands;
using System;
using System.Collections.Generic;

namespace Encoder.Commands
{
    internal class OpenWindowSelectEncodingCommand : CommandBase
    {
        private readonly IDialogService _dialogService;
        private readonly Action<string> _onEncodingSelected;
        private readonly IEnumerable<string> _encodings;

        public OpenWindowSelectEncodingCommand(IDialogService dialogService, Action<string> onEncodingSelected, IEnumerable<string> encodings)
        {
            _dialogService = dialogService;
            _onEncodingSelected = onEncodingSelected;
            _encodings = encodings;
        }

        public override void Execute(object parameter)
        {
            var dialogWindow = _dialogService.GetDialog<SelectEncodingWindow, SelectEncodingWindowViewModel, string>
                (
                    onCloseCallback: OnWindowSelectEncodingClosed,
                    ViewModelparameters: _encodings
                );

            dialogWindow.ShowDialog();
        }

        private void OnWindowSelectEncodingClosed(bool? dialogResult, string selectedEncoding)
        {
            if (dialogResult.HasValue && dialogResult.Value)
            {
                // Если DiaglogResult = OK (т. е. что-то было выбрано

                _onEncodingSelected?.Invoke(selectedEncoding);
            }
        }
    }
}
