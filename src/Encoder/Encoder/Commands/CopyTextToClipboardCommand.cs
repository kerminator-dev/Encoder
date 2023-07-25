using EncodingLibrary.Commands;
using System;
using System.Windows;

namespace Encoder.Commands
{
    internal class CopyTextToClipboardCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (!(parameter is String)) // Старая версия C#, not нельзя добавить
                return;

            string text = (string)parameter;

            if (string.IsNullOrEmpty(text))
                return;

            Clipboard.SetText(text);
        }
    }
}
