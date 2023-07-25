using Encoder.Interfaces;
using EncodingLibrary.ViewModels;
using System;
using System.Windows;

namespace Encoder.Builders
{
    public interface IDialogService
    {
        TView GetDialog<TView, TViewModel, TResult>(Action<bool?, TResult> onCloseCallback, params object[] ViewModelparameters)
         where TView : Window
         where TViewModel : ViewModelBase, IDialogResultOf<TResult>;

        TView GetDialog<TView, TViewModel>()
            where TView : Window
            where TViewModel : ViewModelBase;

        TView GetDialog<TView>()
            where TView : Window;
    }
}
