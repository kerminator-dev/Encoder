using Encoder.Builders;
using Encoder.Views;
using EncodingLibrary.ViewModels;
using EncodingLibrary.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace EncodingLibrary
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        protected void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<SelectEncodingWindow>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = serviceProvider.GetService<MainWindow>();
            MainWindow.DataContext = serviceProvider.GetService<MainWindowViewModel>();
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
