using EncodingLibrary.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;

namespace Encoder.Builders
{
    public class DialogBuilder : IBuilder<Window>
    {
        private readonly IServiceProvider _serviceProvider;

        private Type _viewType;
        private Type _viewModelType;
        private object[] _viewModelParams;

        public DialogBuilder(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public DialogBuilder WithView<TView>() where TView : Window
        {
            _viewType = typeof(TView);

            return this;
        }

        public DialogBuilder WithViewModel<TViewModel>() 
            where TViewModel : ViewModelBase
        {
            _viewModelType = typeof(TViewModel);

            return this;
        }

        public DialogBuilder WithViewModelParams(params object[] parameters) 
        {
            _viewModelParams = parameters;

            return this;
        }

        public Window Build()
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

            return view;
        }
    }
}
