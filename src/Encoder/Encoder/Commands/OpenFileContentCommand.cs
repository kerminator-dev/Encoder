using EncodingLibrary.Commands;
using Microsoft.Win32;
using System;
using System.IO;

namespace Encoder.Commands
{
    internal class OpenFileContentCommand : CommandBase
    {
        private readonly string _filter;
        private readonly Action<Models.FileInfo> _onFileOpened;
        private readonly Action<Exception> _onException;

        public OpenFileContentCommand(Action<Models.FileInfo> onFileOpened, Action<Exception> onException = null, string filter = "Все файлы(*.*)|*.*")
        {
            if (string.IsNullOrEmpty(filter))
                throw new ArgumentNullException(nameof(filter));

            if (onFileOpened == null)
                throw new ArgumentNullException(nameof(onFileOpened));

            _filter = filter;
            _onFileOpened = onFileOpened;
            _onException = onException;
        }

        public async override void Execute(object parameter)
        {
            try
            {
                OpenFileDialog openfileDialog = new OpenFileDialog();
                openfileDialog.Filter = _filter;

                if (openfileDialog.ShowDialog() == true)
                {
                    using (StreamReader reader = new StreamReader(openfileDialog.FileName))
                    {
                        var result = new Models.FileInfo
                        (
                            filename: openfileDialog.FileName,
                            content: await reader.ReadToEndAsync(),
                            encoding: reader.CurrentEncoding.HeaderName
                        );
                        _onFileOpened?.Invoke(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }
        }
    }
}
