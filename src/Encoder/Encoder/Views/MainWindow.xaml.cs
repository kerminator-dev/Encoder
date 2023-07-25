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


        // Если юзать Command CopyTextToClipboadCommand, то 
        // Нужно, чтобы Binding точно отработал (подтверждение ввода или фокус на другой
        // элемент управления UI). Можно сделать посимвольное обновление свойства в MainWindowViewModel, но 
        // Могут возникнуть нюансы с производительностью
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
