using Encoder.Interfaces;
using EncodingLibrary.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Encoder.Services
{
    public class DialogBuilderService
    {
        private readonly IServiceProvider _serviceProvider;

        private Type _viewType;
        private Type _viewModelType;
        private object[] _viewModelParams;

        public DialogBuilderService(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public DialogBuilderService WithView<TView>() where TView : Window
        {
            _viewType = typeof(TView);

            return this;
        }

        public DialogBuilderService WithViewModel<TViewModel>() 
            where TViewModel : ViewModelBase
        {
            _viewModelType = typeof(TViewModel);

            return this;
        }

        public DialogBuilderService WithViewModelParams(params object[] parameters) 
        {
            _viewModelParams = parameters;

            return this;
        }

        public bool? ShowDialog()
        {
            var view = _serviceProvider.GetRequiredService(_viewType) as Window;

            object viewModel = null;

            if (_viewModelParams.Any())
            {
                viewModel = Activator.CreateInstance(_viewModelType, _viewModelParams);
            }
            
            if (viewModel != null)
            {
                view.DataContext = viewModel;
            }

            return view.ShowDialog();
        }
    }
}
