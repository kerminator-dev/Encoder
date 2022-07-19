using Encoder.ViewModels;
using Encoder.Views;
using System.Windows;

namespace Encoder
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
