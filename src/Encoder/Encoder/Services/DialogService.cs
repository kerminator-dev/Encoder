using Encoder.Interfaces;
using EncodingLibrary.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace Encoder.Builders
{
    internal class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider= serviceProvider;
        }

        public TView GetDialog<TView, TViewModel, TResult>(Action<bool?, TResult> onCloseCallback, params object[] ViewModelparameters)
            where TView : Window
            where TViewModel : ViewModelBase, IDialogResultOf<TResult>
        {
            TView view = _serviceProvider.GetRequiredService<TView>();
            var viewModel = Activator.CreateInstance(typeof(TViewModel), ViewModelparameters);

            EventHandler closeEventHandler = default;
            closeEventHandler += (o, e) =>
            {
                onCloseCallback(view.DialogResult, (viewModel as IDialogResultOf<TResult>).GetResult());

                view.Closed -= closeEventHandler;
            };

            view.Closed += closeEventHandler;

            view.DataContext = viewModel;

            return view;
        }

        public TView GetDialog<TView, TViewModel>()
            where TView : Window
            where TViewModel : ViewModelBase
        {
            TView view = _serviceProvider.GetRequiredService<TView>();
            var viewModel = Activator.CreateInstance(typeof(TViewModel));

            view.DataContext = viewModel;
            return view;
        }

        public TView GetDialog<TView>()
            where TView : Window
        {
            return _serviceProvider.GetRequiredService<TView>();
        }
    }
}
