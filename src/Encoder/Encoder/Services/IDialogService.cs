using Encoder.Interfaces;
using EncodingLibrary.ViewModels;
using System;
using System.Windows;

namespace Encoder.Services
{
    public interface IDialogService
    {
        bool? ShowDialog<TView, TViewModel, TResult>(Action<TResult> onCloseCallback, params object[] ViewModelparameters)
         where TView : Window
         where TViewModel : ViewModelBase, IResultOf<TResult>;


        //bool? ShowDialog<TView, TViewModel>()
        //    where TView : Window
        //    where TViewModel : ViewModelBase;

        bool? ShowDialog<TView>()
            where TView : Window;
    }
}
