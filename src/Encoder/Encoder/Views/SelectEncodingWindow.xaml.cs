using System.Windows;

namespace Encoder.Views
{
    /// <summary>
    /// Логика взаимодействия для SelectEncodingWindow.xaml
    /// </summary>
    public partial class SelectEncodingWindow : Window
    {
        public SelectEncodingWindow()
        {
            InitializeComponent();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }
    }
}
