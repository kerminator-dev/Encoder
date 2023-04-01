using Encoder.Interfaces;
using EncodingLibrary;
using EncodingLibrary.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Mvvm.Interfaces;

namespace Encoder.Services
{
    internal class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider= serviceProvider;
        }

        public bool? ShowDialog<TView, TViewModel, TResult>(Action<bool?, TResult> onCloseCallback, params object[] ViewModelparameters)
            where TView : Window
            where TViewModel : ViewModelBase, IResultOf<TResult>
        {
            TView view = _serviceProvider.GetRequiredService<TView>();
            var viewModel = Activator.CreateInstance(typeof(TViewModel), ViewModelparameters);

            EventHandler closeEventHandler = default;
            closeEventHandler += (o, e) =>
            {
                onCloseCallback(view.DialogResult, (viewModel as IResultOf<TResult>).GetResult());

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
            TView view = _serviceProvider.GetRequiredService<TView>();
            var viewModel = Activator.CreateInstance(typeof(TViewModel));

            view.DataContext = viewModel;
            return view.ShowDialog();
        }

        public bool? ShowDialog<TView>()
            where TView : Window
        {
            return _serviceProvider.GetRequiredService<TView>().ShowDialog();
        }
    }
}
