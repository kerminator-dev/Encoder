using Encoder.Interfaces;
using EncodingLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Encoder.ViewModels
{
    public class SelectEncodingWindowViewModel : ViewModelBase, IDialogResultOf<string>
    {
        public IEnumerable<string> Encodings { get; private set; }

        private string _selectedEncoding;
        public string SelectedEncoding
        {
            get => _selectedEncoding;
            set
            {
                _selectedEncoding = value;

                base.OnPropertyChanged(nameof(SelectedEncoding));
                base.OnPropertyChanged(nameof(IsSelected));
            }
        }

        public bool IsSelected => !string.IsNullOrEmpty(SelectedEncoding);

        public SelectEncodingWindowViewModel(IEnumerable<string> encodings)
        {
            if (encodings == null || !encodings.Any())
            {
                throw new ArgumentNullException(nameof(encodings));
            }

            Encodings = encodings;
            SelectedEncoding = encodings.FirstOrDefault();
        }

        public string GetResult()
        {
            return SelectedEncoding;
        }


    }
}
