using EncodingLibrary.ViewModels;
using System.Windows;

namespace Encoder.Services
{
    internal interface IDialogService
    {
        bool? ShowDialog<TView, TViewModel>(params object[] parameters)
            where TView : Window
            where TViewModel : ViewModelBase;

        bool? ShowDialog<TView, TViewModel>()
            where TView : Window
            where TViewModel : ViewModelBase;

        bool? ShowDialog<TView>()
            where TView : Window;
    }
}
