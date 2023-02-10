using Encoder.Interfaces;
using EncodingLibrary;
using EncodingLibrary.ViewModels;
using System;
using System.Security.Cryptography;
using System.Windows;
using Wpf.Ui.Mvvm.Interfaces;

namespace Encoder.Services
{
    internal class DialogService : IDialogService
    {
        public bool? ShowDialog<TView, TViewModel, TResult>(Action<TResult> onCloseCallback, params object[] ViewModelparameters)
            where TView : Window
            where TViewModel : ViewModelBase, IResultOf<TResult>
        {
            TView view = Activator.CreateInstance<TView>();
            var viewModel = Activator.CreateInstance(typeof(TViewModel), ViewModelparameters);

            EventHandler closeEventHandler = default;
            closeEventHandler += (o, e) =>
            {
                onCloseCallback((viewModel as IResultOf<TResult>).GetResult());

                view.Closed -= closeEventHandler;
            };

            view.Closed += closeEventHandler;

            view.DataContext = viewModel;

            return view.ShowDialog();
        }


        public bool? ShowDialog<TView, TViewModel>()
            where TView : Window
            where TViewModel : ViewModelBase
        {
            TView view = Activator.CreateInstance<TView>();
            var viewModel = Activator.CreateInstance(typeof(TViewModel));

            view.DataContext = viewModel;
            return view.ShowDialog();
        }

        public bool? ShowDialog<TView>()
            where TView : Window
        {
            TView view = Activator.CreateInstance<TView>();

            return view.ShowDialog();
        }
    }
}
