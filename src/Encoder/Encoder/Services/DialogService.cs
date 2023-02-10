using EncodingLibrary.ViewModels;
using System;
using System.Security.Cryptography;
using System.Windows;
using Wpf.Ui.Mvvm.Interfaces;

namespace Encoder.Services
{
    internal class DialogService : IDialogService
    {
        public bool? ShowDialog<TView, TViewModel>(params object[] parameters)
            where TView : Window
            where TViewModel : ViewModelBase
        {
            TView view = Activator.CreateInstance<TView>();
            var viewModel = Activator.CreateInstance(typeof(TViewModel), parameters);

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
