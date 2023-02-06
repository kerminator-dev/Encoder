using System.Windows;

namespace EncodingLibrary.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EditMenu_Cancel(object sender, RoutedEventArgs e)
        {
            InputTextBox.Undo();
        }

        private void EditMenu_Paste(object sender, RoutedEventArgs e)
        {
            InputTextBox.Paste();
        }

        private void EditMenu_CopyInputText(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(InputTextBox.Text);
        }

        private void EditMenu_CopyOutputText(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(OutputTextBox.Text);
        }
    }
}
