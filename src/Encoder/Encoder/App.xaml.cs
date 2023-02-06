﻿using EncodingLibrary.ViewModels;
using EncodingLibrary.Views;
using System.Windows;

namespace EncodingLibrary
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
