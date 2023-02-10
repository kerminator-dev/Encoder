using Encoder.Interfaces;
using EncodingLibrary;
using EncodingLibrary.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoder.ViewModels
{
    public class SelectEncodingWindowViewModel : ViewModelBase, IResultOf<string>
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
            Encodings = encodings;
            SelectedEncoding = Encodings.FirstOrDefault();
        }

        public string GetResult()
        {
            return SelectedEncoding;
        }
    }
}
