using System.Windows;

namespace ProjectWPF.StudentManage
{
    public partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 